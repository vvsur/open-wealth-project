using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace owp.FDownloader
{
    public partial class FinamTreeView : TreeView
    {
        public FinamTreeView()
        {
            InitializeComponent();
            base.ImageList = imageList;
        }

        #region Логика Checked
        /// <summary>
        /// Изменяю статус заданной node
        /// </summary>
        /// <param name="node">TreeNode для изменения статуса</param>
        private void ChangeNodeState(TreeNode node)
        {
            if (node == null)
                return;

            BeginUpdate();
            if ((node.Tag as EmitentInfo).id != -1)
            {
                //node является инструментом (есть только два статуса)
                if ((node.ImageIndex == 0) || (node.ImageIndex < 0))
                {
                    node.ImageIndex = 1;
                }
                else
                {
                    node.ImageIndex = 0;
                }
                ChangeNodeState(node.Parent);
            }
            else
            {
                // node является рынком, просто рассчитываю нужную картинку
                // позволять выделять целый рынок одним кликом не хочется, финам жалко
                bool isChecked = false;
                bool allChecked = true;
                foreach (TreeNode child in node.Nodes)
                {
                    isChecked = (isChecked | (child.ImageIndex == 1));
                    allChecked = (allChecked & (child.ImageIndex == 1));
                }
                if (allChecked)
                    node.ImageIndex = 1;
                else
                    if (isChecked)
                        node.ImageIndex = 2;
                    else
                        node.ImageIndex = 0;
            }

            node.SelectedImageIndex = node.ImageIndex;
            EndUpdate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            TreeNode node = GetNodeAt(PointToClient(Control.MousePosition));
            ChangeNodeState(node);
            base.OnMouseDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Space)
                ChangeNodeState(SelectedNode);
        }
        #endregion

        /// <summary>
        /// Возвращает список инструментов, присутствующих в TreeView
        /// </summary>
        /// <returns>список инструментов</returns>
        public List<EmitentInfo> GetEmitents()
        {
            List<EmitentInfo> emitents = new List<EmitentInfo>();
            foreach (TreeNode m in Nodes)
            {
                //settings.Emitents.Add((m.Tag as EmitentInfo)); т.к. данная инфа избыточна
                foreach (TreeNode i in m.Nodes)
                {
                    (i.Tag as EmitentInfo).checed = (i.ImageIndex == 1);
                    emitents.Add((i.Tag as EmitentInfo));
                }
            }
            return emitents;
        }
        /// <summary>
        /// Используется в SetEmitents для добавления инструмента в заданный узел-секцию
        /// </summary>
        /// <param name="node">узел-секция куда будет добавлен инструмент</param>
        /// <param name="emitent">инструмент для добавления</param>
        private void AddEmitent(TreeNode node, EmitentInfo emitent)
        {
            TreeNode i = node.Nodes.Add(emitent.name);
            i.Tag = emitent;
            if (emitent.checed)
            {
                i.ImageIndex = 1;
                i.SelectedImageIndex = 1;
            }
        }
        /// <summary>
        /// Заполнение TreeView данными из settings.Emitents
        /// </summary>
        /// <param name="emitents">список инструментов</param>
        public void SetEmitents(List<EmitentInfo> emitents)
        {
            Nodes.Clear();
            BeginUpdate();
            TreeNode currentMarket = null; // запоминаю узел-секцию, с которой работаю сейчас
            foreach (EmitentInfo emitent in emitents)
            {
                if (currentMarket != null)
                {
                    if ((currentMarket.Tag as EmitentInfo).marketId == emitent.marketId)
                    {
                        AddEmitent(currentMarket, emitent);
                        continue;
                    }
                }
                // пытаюсь найти секцию
                currentMarket = null;
                foreach (TreeNode m in Nodes)
                    if ((m.Tag as EmitentInfo).marketId == emitent.marketId)
                    {
                        currentMarket = m;
                        AddEmitent(currentMarket, emitent);
                        break;
                    }
                // создаю секцию, если её ранее небыло
                if (currentMarket == null)
                {
                    currentMarket = Nodes.Add(emitent.marketName);
                    currentMarket.Tag = new EmitentInfo(emitent.marketId, emitent.marketName, -1, String.Empty, String.Empty);
                    AddEmitent(currentMarket, emitent);
                }
            }
            // устанавливаю статус для секций
            foreach (TreeNode m in Nodes)
            {
                ChangeNodeState(m);
            }
            EndUpdate();
        }
    }
}
