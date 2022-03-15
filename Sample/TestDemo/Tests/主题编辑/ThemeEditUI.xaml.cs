using ay.Animate;
using ay.contents;
using ay.Controls;
using ay.Controls.Info;
using ay.Wpf.Theme.Element.Common;
using Ay.Framework.DataCreaters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestDemo
{
    /// <summary>
    /// ThemeEditUI.xaml 的交互逻辑
    /// </summary>
    public partial class ThemeEditUI : UserControl
    {
        public ThemeEditUI()
        {
            InitializeComponent();
            Loaded += ThemeEditUI_Loaded;
        }

        public ObservableCollection<AyTreeViewItemModel> CreateMenuTreeData()
        {
            ObservableCollection<AyTreeViewItemModel> list = new ObservableCollection<AyTreeViewItemModel>();
            AyTreeViewItemModel root_0 = new AyTreeViewItemModel("多级节点", "fa-home", null, false);

            AyTreeViewItemModel root_01 = new AyTreeViewItemModel("多级节点1", "fa-home", root_0, false);
            AyTreeViewItemModel root_0101 = new AyTreeViewItemModel("多级节点1_1", "fa-birthday-cake", root_01, false);
            AyTreeViewItemModel root_0102 = new AyTreeViewItemModel("多级节点1_2", "fa-git", root_01, false);
            AyTreeViewItemModel root_0103 = new AyTreeViewItemModel("多级节点1_3", "fa-chain-broken", root_01, false);
            AyTreeViewItemModel root_010301 = new AyTreeViewItemModel("多级节点1_3_1", "fa-check", root_0103, false);
            AyTreeViewItemModel root_010302 = new AyTreeViewItemModel("多级节点1_3_2", "fa-check-square-o", root_0103, false);

            AyTreeViewItemModel root_02 = new AyTreeViewItemModel("多级节点2", "fa-copy", root_0, false);
            AyTreeViewItemModel root_03 = new AyTreeViewItemModel("多级节点3", "fa-comments", root_0, false);

            AyTreeViewItemModel root = new AyTreeViewItemModel("整体Demo1", "fa-align-justify", null, false, "/Demo/MainControlPage.xaml");

            AyTreeViewItemModel root_1 = new AyTreeViewItemModel("整体Demo2", "fa-align-justify", null, false, "/Demo/MainControlPage2.xaml");

            AyTreeViewItemModel root0 = new AyTreeViewItemModel("容器控件", "fa-briefcase", null, false);
            AyTreeViewItemModel root0_1 = new AyTreeViewItemModel("AyTabControl", "fa-circle-o", root0, false, "/Demo/TabControlPage.xaml");
            AyTreeViewItemModel root0_2 = new AyTreeViewItemModel("AyWindow", "fa-circle-o", root0, false);
            AyTreeViewItemModel root0_3 = new AyTreeViewItemModel("AyPopupWindow", "fa-circle-o", root0, false);
            AyTreeViewItemModel root0_4 = new AyTreeViewItemModel("AyPanel", "fa-circle-o", root0, false);
            AyTreeViewItemModel root0_5 = new AyTreeViewItemModel("AyWrapPanel", "fa-circle-o", root0, false);
            AyTreeViewItemModel root0_7 = new AyTreeViewItemModel("AyPagePanel", "fa-circle-o", root0, false);
            AyTreeViewItemModel root0_6 = new AyTreeViewItemModel("AyRadioList", "fa-circle-o", root0, false);
            AyTreeViewItemModel root0_8 = new AyTreeViewItemModel("AyCheckList", "fa-circle-o", root0, false);

            AyTreeViewItemModel root1 = new AyTreeViewItemModel("基本控件", "fa-table", null, false);

            AyTreeViewItemModel root1_0 = new AyTreeViewItemModel("Button", "fa-circle-o", root1, false);
            AyTreeViewItemModel root1_9 = new AyTreeViewItemModel("ToggleButton", "fa-circle-o", root1, false);
            AyTreeViewItemModel root1_13 = new AyTreeViewItemModel("AySplitButton", "fa-circle-o", root1, false);
            AyTreeViewItemModel root1_14 = new AyTreeViewItemModel("AyComboMenu", "fa-circle-o", root1, false);
            AyTreeViewItemModel root1_15 = new AyTreeViewItemModel("AyStrokeLabel", "fa-circle-o", root1, false);

            AyTreeViewItemModel root1_12 = new AyTreeViewItemModel("AySwitch", "fa-circle-o", root1, false, "/Demo/AySwitchPage.xaml");
            AyTreeViewItemModel root1_10 = new AyTreeViewItemModel("AyImage*Button", "fa-circle-o", root1, false);
            AyTreeViewItemModel root1_11 = new AyTreeViewItemModel("AyPath", "fa-circle-o", root1, false);
            AyTreeViewItemModel root1_1 = new AyTreeViewItemModel("AyTextBox", "fa-circle-o", root1, false);
            AyTreeViewItemModel root1_2 = new AyTreeViewItemModel("AyAutoComplete", "fa-circle-o", root1, false);
            AyTreeViewItemModel root1_3 = new AyTreeViewItemModel("AyComboBox", "fa-circle-o", root1, false);
            AyTreeViewItemModel root1_4 = new AyTreeViewItemModel("AyIconAll", "fa-circle-o", root1, false);
            AyTreeViewItemModel root1_5 = new AyTreeViewItemModel("AySlider", "fa-circle-o", root1, false, "/Demo/AySliderPage.xaml");
            AyTreeViewItemModel root1_16 = new AyTreeViewItemModel("AyOpacitySetSlider", "fa-circle-o", root1, false);
            AyTreeViewItemModel root1_6 = new AyTreeViewItemModel("AyTextBox", "fa-circle-o", root1, false);
            AyTreeViewItemModel root1_7 = new AyTreeViewItemModel("AyColorPicker", "fa-circle-o", root1, false);
            AyTreeViewItemModel root1_8 = new AyTreeViewItemModel("AyColorPickerDialog", "fa-circle-o", root1, false);

            AyTreeViewItemModel root3 = new AyTreeViewItemModel("数据展示", "fa-joomla", null, false);
            AyTreeViewItemModel root3_1 = new AyTreeViewItemModel("AyTreeView", "fa-circle-o", root3, false);
            AyTreeViewItemModel root3_2 = new AyTreeViewItemModel("AySimplyListView", "fa-circle-o", root3, false, "/Demo/AySimplyListViewPage.xaml");

            AyTreeViewItemModel root4 = new AyTreeViewItemModel("特殊展示", "fa-list-alt", null, false);
            AyTreeViewItemModel root4_0 = new AyTreeViewItemModel("AyArcChart", "fa-circle-o", root4, false, "/Demo/AyArcChartPage.xaml");
            AyTreeViewItemModel root4_1 = new AyTreeViewItemModel("AyImageViewer", "fa-circle-o", root4, false, "/Demo/AyImageViewerPage.xaml");
            AyTreeViewItemModel root4_2 = new AyTreeViewItemModel("ShowCurrentTime", "fa-circle-o", root4, false, "/Demo/TimePage.xaml");
            AyTreeViewItemModel root4_3 = new AyTreeViewItemModel("Uc_HKControl", "fa-circle-o", root4, false, "/Demo/PieFourButtonPage.xaml");
            AyTreeViewItemModel root4_4 = new AyTreeViewItemModel("海康监控示例1", "fa-circle-o", root4, false, "/Demo/HaiKangPage.xaml");
            AyTreeViewItemModel root4_5 = new AyTreeViewItemModel("海康监控示例2", "fa-circle-o", root4, false, "/Demo/HaiKangPageLoginView.xaml");
            AyTreeViewItemModel root5 = new AyTreeViewItemModel("版本说明", "fa-info-circle", null, false);


            list.Add(root_0);
            list.Add(root);
            list.Add(root_1);
            list.Add(root0);
            list.Add(root1);
            list.Add(root3);
            list.Add(root4);

            return list;
        }

        public ObservableCollection<TestTokenItem> TestTokenItems { get; set; }
        AySportsViewModel vmSport = new AySportsViewModel();

        //SearchMemberPaths="Text,Value"
        private void ThemeEditUI_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= ThemeEditUI_Loaded;
            svRoot.ScrollToVerticalOffset(2000);

            TestTokenItems = new ObservableCollection<TestTokenItem>();
            //创建集合
            TestTokenItems.Add(new TestTokenItem { Text = "阿坝", Value = "0000001" });
            TestTokenItems.Add(new TestTokenItem { Text = "安庆", Value = "0000002" });
            TestTokenItems.Add(new TestTokenItem { Text = "澳门", Value = "0000003" });
            TestTokenItems.Add(new TestTokenItem { Text = "北京", Value = "0000004" });
            TestTokenItems.Add(new TestTokenItem { Text = "包头", Value = "0000005" });
            TestTokenItems.Add(new TestTokenItem { Text = "亳州", Value = "0000006" });
            TestTokenItems.Add(new TestTokenItem { Text = "重庆", Value = "0000007" });
            TestTokenItems.Add(new TestTokenItem { Text = "成都", Value = "0000008" });
            TestTokenItems.Add(new TestTokenItem { Text = "阿拉善", Value = "0000009" });
            TestTokenItems.Add(new TestTokenItem { Text = "大连", Value = "00000010" });
            TestTokenItems.Add(new TestTokenItem { Text = "德州", Value = "00000011" });
            TestTokenItems.Add(new TestTokenItem { Text = "鄂尔多斯", Value = "00000012" });
            TestTokenItems.Add(new TestTokenItem { Text = "福州", Value = "00000013" });
            TestTokenItems.Add(new TestTokenItem { Text = "广州", Value = "00000014" });
            TestTokenItems.Add(new TestTokenItem { Text = "哈尔滨", Value = "0000015" });
            TestTokenItems.Add(new TestTokenItem { Text = "合肥", Value = "0000016" });
            TestTokenItems.Add(new TestTokenItem { Text = "济南", Value = "00000017" });
            TestTokenItems.Add(new TestTokenItem { Text = "昆明", Value = "00000018" });
            TestTokenItems.Add(new TestTokenItem { Text = "拉萨", Value = "00000019" });
            TestTokenItems.Add(new TestTokenItem { Text = "连云港", Value = "00000020" });
            TestTokenItems.Add(new TestTokenItem { Text = "马鞍山", Value = "00000021" });
            TestTokenItems.Add(new TestTokenItem { Text = "南京", Value = "00000022" });
            TestTokenItems.Add(new TestTokenItem { Text = "南昌", Value = "00000023" });
            TestTokenItems.Add(new TestTokenItem { Text = "南通", Value = "00000024" });
            TestTokenItems.Add(new TestTokenItem { Text = "攀枝花", Value = "00000025" });
            TestTokenItems.Add(new TestTokenItem { Text = "青岛", Value = "00000026" });
            TestTokenItems.Add(new TestTokenItem { Text = "上海", Value = "00000027" });
            TestTokenItems.Add(new TestTokenItem { Text = "天津", Value = "00000028" });
            TestTokenItems.Add(new TestTokenItem { Text = "厦门", Value = "00000029" });
            TestTokenItems.Add(new TestTokenItem { Text = "银川", Value = "00000030" });
            TestTokenItems.Add(new TestTokenItem { Text = "孝感", Value = "00000031" });
            TestTokenItems.Add(new TestTokenItem { Text = "铜陵", Value = "00000032" });
            TestTokenItems.Add(new TestTokenItem { Text = "延安", Value = "00000033" });
            TestTokenItems.Add(new TestTokenItem { Text = "银川", Value = "00000034" });
            TestTokenItems.Add(new TestTokenItem { Text = "香港", Value = "00000035" });
            TestTokenItems.Add(new TestTokenItem { Text = "锡林郭勒", Value = "00000036" });
            TestTokenItems.Add(new TestTokenItem { Text = "无锡", Value = "00000037" });
            TestTokenItems.Add(new TestTokenItem { Text = "廊坊", Value = "00000038" });
            ttxtDiy.ItemsSource = allowInVliadTokenTextBox.ItemsSource = allowDuTokenTextBox.ItemsSource = defaultTokenizedTextBox.ItemsSource = TestTokenItems;



            ObservableCollection<AyPerson> TableViewDatas = new ObservableCollection<AyPerson>();
            for (int i = 0; i < 100; i++)
            {
                AyPerson Model = new AyPerson();
                if (i == 0)
                {
                    Model.Name = "杨洋AY";
                }
                else
                {
                    Model.Name = AyUserName.UserName();
                }
                Model.Sex = AyCommon.Rnd.Next(5);
                Model.Telphone = AyPhone.PhoneNumber();
                Model.Address = AyAddress.Address();
                TableViewDatas.Add(Model);
            }

            tableView.ItemsSource = TableViewDatas;



            #region 测试数据 RadioBoxList CheckBoxList


            AyCheckBoxListItemModel sm1 = new AyCheckBoxListItemModel();
            sm1.ItemText = "羽毛球";
            sm1.ItemValue = "ymq";
            sm1.IsChecked = false;

            AyCheckBoxListItemModel sm2 = new AyCheckBoxListItemModel();
            sm2.ItemText = "乒乓球";
            sm2.ItemValue = "ppq";
            sm2.IsChecked = false;

            AyCheckBoxListItemModel sm3 = new AyCheckBoxListItemModel();
            sm3.ItemText = "游泳";
            sm3.ItemValue = "yy";
            sm3.IsChecked = false;

            AyCheckBoxListItemModel sm4 = new AyCheckBoxListItemModel();
            sm4.ItemText = "跑步";
            sm4.ItemValue = "pb";
            sm4.IsChecked = false;




            vmSport.sports0.Add(sm1);
            vmSport.sports0.Add(sm2);
            vmSport.sports0.Add(sm3);
            vmSport.sports0.Add(sm4);




            AyCheckBoxListItemModel m1 = new AyCheckBoxListItemModel();
            m1.ItemText = "羽毛球";
            m1.ItemValue = "ymq";
            m1.IsChecked = false;

            AyCheckBoxListItemModel m2 = new AyCheckBoxListItemModel();
            m2.ItemText = "乒乓球";
            m2.ItemValue = "ppq";
            m2.IsChecked = false;

            AyCheckBoxListItemModel m3 = new AyCheckBoxListItemModel();
            m3.ItemText = "游泳";
            m3.ItemValue = "yy";
            m3.IsChecked = false;

            AyCheckBoxListItemModel m4 = new AyCheckBoxListItemModel();
            m4.ItemText = "跑步";
            m4.ItemValue = "pb";
            m4.IsChecked = false;

            AyCheckBoxListItemModel m5 = new AyCheckBoxListItemModel();
            m5.ItemText = "举重";
            m5.ItemValue = "jz";
            m5.IsChecked = false;

            AyCheckBoxListItemModel m6 = new AyCheckBoxListItemModel();
            m6.ItemText = "平板撑";
            m6.ItemValue = "pbc";
            m6.IsChecked = false;


            AyCheckBoxListItemModel m61 = new AyCheckBoxListItemModel();
            m61.ItemText = "平板撑1";
            m61.ItemValue = "pbc1";
            m61.IsChecked = false;

            AyCheckBoxListItemModel m62 = new AyCheckBoxListItemModel();
            m62.ItemText = "平板撑2";
            m62.ItemValue = "pbc2";
            m62.IsChecked = true;

            AyCheckBoxListItemModel m63 = new AyCheckBoxListItemModel();
            m63.ItemText = "平板撑3";
            m63.ItemValue = "pbc3";
            m63.IsChecked = true;

            AyCheckBoxListItemModel m64 = new AyCheckBoxListItemModel();
            m64.ItemText = "平板撑4";
            m64.ItemValue = "pbc4";
            m64.IsChecked = false;

            AyCheckBoxListItemModel m65 = new AyCheckBoxListItemModel();
            m65.ItemText = "平板撑5";
            m65.ItemValue = "pbc5";
            m65.IsChecked = false;

            AyCheckBoxListItemModel m66 = new AyCheckBoxListItemModel();
            m66.ItemText = "平板撑6";
            m66.ItemValue = "pbc6";
            m66.IsChecked = false;

            AyCheckBoxListItemModel m67 = new AyCheckBoxListItemModel();
            m67.ItemText = "平板撑7";
            m67.ItemValue = "pbc7";
            m67.IsChecked = false;

            vmSport.sports.Add(m1);
            vmSport.sports.Add(m2);
            vmSport.sports.Add(m3);
            vmSport.sports.Add(m4);
            vmSport.sports.Add(m5);
            vmSport.sports.Add(m6);

            vmSport.sports.Add(m61);
            vmSport.sports.Add(m62);
            vmSport.sports.Add(m63);
            vmSport.sports.Add(m64);
            vmSport.sports.Add(m65);
            vmSport.sports.Add(m66);
            vmSport.sports.Add(m67);

            AyCheckBoxListItemModel rm1 = new AyCheckBoxListItemModel();
            rm1.ItemText = "爸爸";
            rm1.ItemValue = "bb";
            rm1.IsChecked = false;

            AyCheckBoxListItemModel rm2 = new AyCheckBoxListItemModel();
            rm2.ItemText = "妈妈";
            rm2.ItemValue = "mm";
            rm2.IsChecked = false;

            AyCheckBoxListItemModel rm3 = new AyCheckBoxListItemModel();
            rm3.ItemText = "妹妹";
            rm3.ItemValue = "meimei";
            rm3.IsChecked = false;

            AyCheckBoxListItemModel rm4 = new AyCheckBoxListItemModel();
            rm4.ItemText = "弟弟";
            rm4.ItemValue = "dd";
            rm4.IsChecked = false;

            AyCheckBoxListItemModel rm5 = new AyCheckBoxListItemModel();
            rm5.ItemText = "嫂子";
            rm5.ItemValue = "sz";
            rm5.IsChecked = false;

            AyCheckBoxListItemModel rm6 = new AyCheckBoxListItemModel();
            rm6.ItemText = "奶奶";
            rm6.ItemValue = "nn";
            rm6.IsChecked = false;

            vmSport.sports2.Add(rm1);
            vmSport.sports2.Add(rm2);
            vmSport.sports2.Add(rm3);
            vmSport.sports2.Add(rm4);
            vmSport.sports2.Add(rm5);
            vmSport.sports2.Add(rm6);


            AyCheckBoxListItemModel rm7 = new AyCheckBoxListItemModel();
            rm7.ItemText = "爸爸2";
            rm7.ItemValue = "bb";
            rm7.IsChecked = false;

            AyCheckBoxListItemModel rm8 = new AyCheckBoxListItemModel();
            rm8.ItemText = "妈妈2";
            rm8.ItemValue = "mm";
            rm8.IsChecked = false;

            AyCheckBoxListItemModel rm9 = new AyCheckBoxListItemModel();
            rm9.ItemText = "妹妹2";
            rm9.ItemValue = "meimei";
            rm9.IsChecked = false;

            AyCheckBoxListItemModel rm10 = new AyCheckBoxListItemModel();
            rm10.ItemText = "弟弟2";
            rm10.ItemValue = "dd";
            rm10.IsChecked = false;

            AyCheckBoxListItemModel rm11 = new AyCheckBoxListItemModel();
            rm11.ItemText = "嫂子2";
            rm11.ItemValue = "sz";
            rm11.IsChecked = false;

            AyCheckBoxListItemModel rm12 = new AyCheckBoxListItemModel();
            rm12.ItemText = "奶奶2";
            rm12.ItemValue = "nn";
            rm12.IsChecked = false;

            vmSport.sports3.Add(rm7);
            vmSport.sports3.Add(rm8);
            vmSport.sports3.Add(rm9);
            vmSport.sports3.Add(rm10);
            vmSport.sports3.Add(rm11);
            vmSport.sports3.Add(rm12);

            vmSport.Submit = (x) =>
            {
                MessageBox.Show("你喜爱的运动项目" + vmSport.LoveSports + " || check：" + vmSport.TestCheckBoxValue);
            };
            gd.DataContext = vmSport;
            #endregion


            tvAuthTree.ItemsSource = CreateMenuTreeData();
        }

        private void BtnGet1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(mtxt1.Text);
        }

        private void BtnGet2_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(mtxt2.Text);
        }

        private void BtnGet3_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(mtxt2.Value?.ToString());
        }

        private void BtnGetTokenText_Click(object sender, RoutedEventArgs e)
        {
            var _1 = allowDuTokenTextBox.SelectedItems;
            StringBuilder sb = new StringBuilder();
            foreach (TestTokenItem item in _1)
            {
                sb.AppendLine(item.Text + "-" + item.Value);
            }
            MessageBox.Show(sb.ToString());
        }




        private void OnDeleteToken(object sender, RoutedEventArgs e)
        {
            object item = ((FrameworkElement)e.OriginalSource).DataContext;
            this.ttxtDiy.SelectedItems.Remove(item);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("触发右侧");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("触发左侧");
        }

        private void BtnGoURI_Click(object sender, RoutedEventArgs e)
        {
            if (cboURLS.SelectedIndex == -1) return;
            var _1 = (ComboBoxItem)cboURLS.SelectedItem;
            System.Diagnostics.Process.Start(_1.Content.ToString());
        }

        private void BtnSearchOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(txtOrderNumber.Text);
        }
        private void btnApplyDateRule_Click(object sender, RoutedEventArgs e)
        {
            dbc.DateRule = cusDateRule.Text;
        }

        private void BtnTestDate1_Click(object sender, RoutedEventArgs e)
        {
            dbc.DateRule = "isShowWeek:true,doubleCalendar:true,dateFmt:'yyyy-MM-dd HH:mm:ss',minDate:'2018-01-01 00:00:00',maxDate:'2019-12-31 23:59:59'";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            dbc.Icon = Icons.path_element_star;
        }




        private void openMessageBox_Click(object sender, RoutedEventArgs e)
        {
            if (cboMsgType.Text == "信息")
            {
                AyMessageBox.ShowInformation("必须填写用户名");
            }
            else if (cboMsgType.Text == "警告")
            {
                AyMessageBox.ShowWarning("必须填写用户名");
            }
            else if (cboMsgType.Text == "错误")
            {
                AyMessageBox.ShowError("必须填写用户名");
            }
            else if (cboMsgType.Text == "异常")
            {
                AyMessageBox.ShowError("必须填写用户名");
            }
            else if (cboMsgType.Text == "疑问")
            {
                AyMessageBox.ShowQuestion("必须填写用户名");
            }
            else if (cboMsgType.Text == "长文本内容")
            {
                AyMessageBox.ShowInformation("我是AY,首先声明,我在做一件很枯燥的事情,我是个91后程序员，每天熬夜完成计划的过着下班后的生活\n" +
         "那天有人反对，那天有人安慰，那天有人嘲讽，那天有人祝福。\n"
       + "过了6个月后，我对自己的梦想一直没有改变过，继续坚持，终于，AYUI诞生了。\n"
       + "今天有人说造轮子，今天有人说你好厉害，今天有人说开源吗 ? 有人说好喜欢...\n"
       + "有贬有褒，但是好的声音多了。\n"
       + "但是身体的各种问题也来了..");
            }
            else if (cboMsgType.Text == "DIY的弹窗")
            {
                AyMessageBox.Show("我是AY,首先声明,我在做一件很枯燥的事情,我是个91后程序员，每天熬夜完成计划的过着下班后的生活\n" +
"那天有人反对，那天有人安慰，那天有人嘲讽，那天有人祝福。\n"
+ "过了6个月后，我对自己的梦想一直没有改变过，继续坚持，终于，AYUI诞生了。\n"
+ "今天有人说造轮子，今天有人说你好厉害，今天有人说开源吗 ? 有人说好喜欢...\n"
+ "有贬有褒，但是好的声音多了。\n"
+ "但是身体的各种问题也来了..", "AY自我介绍", MessageBoxButton.OK, AyMessageBoxImage.None, MessageBoxResult.Yes, MessageBoxOptions.RightAlign);
            }
            else if (cboMsgType.Text == "长文本内容")
            {
            }
            else if (cboMsgType.Text == "删除")
            {
                if (MessageBoxResult.OK == AyMessageBox.ShowDelete("确认删除吗", "删除"))
                {
                    AyMessageBox.ShowRight("操作成功!");
                }
                else
                {
                    AyMessageBox.ShowRight("操作已经被取消!", "操作");
                }
            }
            else if (cboMsgType.Text == "疑问确认取消")
            {
                if (MessageBoxResult.OK == AyMessageBox.ShowQuestionOkCancel("确认操作吗？此操作不可以更改", "操作提示"))
                {
                    AyMessageBox.ShowRight("操作成功!");
                }
                else
                {
                    AyMessageBox.ShowRight("操作已经被取消!", "操作");
                }
            }

            else if (cboMsgType.Text == "ok")
            {
                AyMessageBox.ShowRight("操作成功!");
            }
        }

        private void openIconMessageBox_Click(object sender, RoutedEventArgs e)
        {
            AyMessageBox.ShowCus("确认删除吗", "", "pack://application:,,,/ay.contents;component/Content/Icon/Image/ay/1.png");
        }

        private void openPromt_Click(object sender, RoutedEventArgs e)
        {
            AyMessageBox.Promt((text) =>
            {
                AyMessageBox.ShowInformation("你刚刚输入的是:{0}".StringFormat(text));
            },
           "等待你的输入...");
        }


        //private void inf_scroll(object sender, ScrollChangedEventArgs e)
        //{
        //    for (int i = 0; i < e.VerticalChange; i++)
        //    {
        //        object tmp = lv.Items[0];
        //        lv.Items.RemoveAt(0);
        //        lv.Items.Add(tmp);
        //    }
        //    for (int i = 0; i > e.VerticalChange; i--)
        //    {
        //        object tmp = lv.Items[lv.Items.Count - 1];
        //        lv.Items.RemoveAt(lv.Items.Count - 1);
        //        lv.Items.Insert(0, tmp);
        //    }
        //    lv.ScrollChanged -= inf_scroll;        // remove the handler temporarily
        //    sv.ScrollToVerticalOffset(sv.VerticalOffset - e.VerticalChange);
        //    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Send, new Action(() => {
        //        sv.ScrollChanged += inf_scroll;    // add the handler back after the scrolling has occurred to avoid recursive scrolling
        //    }));
        //}
    }



    public class AyPerson : AyTableViewRowModel
    {
        private string _Name;

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { Set(ref _Name, value); }
        }
        private bool _GetDaXue;

        /// <summary>
        /// 上过大学
        /// </summary>
        public bool GetDaXue
        {
            get { return _GetDaXue; }
            set { Set(ref _GetDaXue, value); }
        }


        private int _Sex;

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex
        {
            get { return _Sex; }
            set { Set(ref _Sex, value); }
        }

        private string _Address;

        /// <summary>
        /// 地址
        /// </summary>
        public string Address
        {
            get { return _Address; }
            set { Set(ref _Address, value); }
        }

        private string _Telphone;

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Telphone
        {
            get { return _Telphone; }
            set { Set(ref _Telphone, value); }
        }

        private double _ShouRu;

        /// <summary>
        /// 收入
        /// </summary>
        public double ShouRu
        {
            get { return _ShouRu; }
            set { Set(ref _ShouRu, value); }
        }


        //
        /// <summary>
        /// 无注释
        /// </summary>
        public ICommand ContextEditItem { get; set; }
        public ICommand ContextRemoveItem { get; set; }
        public ICommand ContextDetailItem { get; set; }

    }


    public class TestTokenItem : ThemeNotifyModel
    {
        private string _Text;

        /// <summary>
        /// 未填写
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set { Set(ref _Text, value, nameof(Text)); }
        }
        private string _Value;

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { return _Value; }
            set { Set(ref _Value, value, nameof(Value)); }
        }


        //当然这里还可以其他值
    }

    public class AySportsViewModel : AyPropertyChanged
    {

        private string _LoveSports;

        /// <summary>
        /// 未填写
        /// </summary>
        public string LoveSports
        {
            get { return _LoveSports; }
            set { Set(ref _LoveSports, value); }
        }

        private string _TestCheckBoxValue;

        /// <summary>
        /// 未填写
        /// </summary>
        public string TestCheckBoxValue
        {
            get { return _TestCheckBoxValue; }
            set { Set(ref _TestCheckBoxValue, value); }
        }

        private ObservableCollection<IAyCheckedItem> _sports0 = new ObservableCollection<IAyCheckedItem>();

        /// <summary>
        /// 未填写
        /// </summary>
        public ObservableCollection<IAyCheckedItem> sports0
        {
            get { return _sports0; }
            set { Set(ref _sports0, value); }
        }

        private ObservableCollection<IAyCheckedItem> _sports = new ObservableCollection<IAyCheckedItem>();

        /// <summary>
        /// 未填写
        /// </summary>
        public ObservableCollection<IAyCheckedItem> sports
        {
            get { return _sports; }
            set { Set(ref _sports, value); }
        }


        private ObservableCollection<IAyCheckedItem> _sports2 = new ObservableCollection<IAyCheckedItem>();

        /// <summary>
        /// 未填写
        /// </summary>
        public ObservableCollection<IAyCheckedItem> sports2
        {
            get { return _sports2; }
            set { Set(ref _sports2, value); }
        }


        private ObservableCollection<IAyCheckedItem> _sports3 = new ObservableCollection<IAyCheckedItem>();

        /// <summary>
        /// 未填写
        /// </summary>
        public ObservableCollection<IAyCheckedItem> sports3
        {
            get { return _sports3; }
            set { Set(ref _sports3, value); }
        }




        public ActionResult Submit { get; set; }
    }
}
