//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ru.novolabs.SuperCore.LimsBusinessObjects;
//using System.Web.UI.HtmlControls;

//namespace ru.novolabs.SuperCore.WebCore
//{
//    public class OrderFormBuilder
//    {
//        private OrderForm form;

//        public OrderFormBuilder(OrderForm form)
//        {
//            this.form = form;
//        }

//        public void Build(HtmlTable table)
//        {

//            for (Int32 y = 0; y < table.Rows.Count; y++)
//            {
//                for (Int32 x = 0; x < table.Rows[y].Cells.Count; x++)
//                {
//                    OrderFormCell cell = form.Cells[x, y];
//                    if ((cell != null) && (cell.Content != null))
//                        BuildCell(table.Rows[y].Cells[x], cell.Content, x, y);
//                }
//            }
//        }

//        private void BuildCell(HtmlTableCell cell, OrderFormCellContent content, Int32 x, Int32 y)
//        {

//            HtmlInputCheckBox check = new HtmlInputCheckBox();
//            cell.Controls.Add(check);
//            check.ID = String.Format("Check_{0}_{1}", new Object[] { x, y });
//            check.Attributes.Add("runat", "server");


//            HtmlGenericControl label = new HtmlGenericControl("label");
//            cell.Controls.Add(label);
//            label.ID = String.Format("Label_{0}_{1}", new Object[] { x, y });
//            label.Attributes.Add("for", check.ID);
//            label.InnerText = content.Caption;

//            HtmlInputHidden bm = new HtmlInputHidden();
//            cell.Controls.Add(bm);
//            bm.ID = String.Format("BM_{0}_{1}", new Object[] { x, y });
//            bm.Value = content.BiomaterialCode;

//            HtmlInputHidden target = new HtmlInputHidden();
//            cell.Controls.Add(target);
//            target.ID = String.Format("Target_{0}_{1}", new Object[] { x, y });
//            target.Value = content.TargetCode;

//            HtmlInputHidden parents = new HtmlInputHidden();
//            cell.Controls.Add(parents);
//            parents.ID = String.Format("Parents_{0}_{1}", new Object[] { x, y });
//            parents.Value = form.GetParentsString(x, y);

//            HtmlInputHidden children = new HtmlInputHidden();
//            cell.Controls.Add(children);
//            children.ID = String.Format("Children_{0}_{1}", new Object[] { x, y });
//            children.Value = form.GetChildrenString(x, y);



//            check.Attributes.Add("onclick", String.Format("javascript:checkClick({0},{1});", new Object[] { x, y }));
//            cell.Attributes.Add("onclick", String.Format("javascript:cellClick({0},{1});", new Object[] { x, y }));
//            label.Attributes.Add("onclick", String.Format("javascript:cellClick({0},{1});", new Object[] { x, y }));

//        }

//    }
//}
