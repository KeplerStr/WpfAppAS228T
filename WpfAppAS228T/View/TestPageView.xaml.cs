using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAppAS228T.ViewModel;

namespace WpfAppAS228T.View
{
    /// <summary>
    /// TestPageView.xaml 的交互逻辑
    /// </summary>
    /// 

    class Test
    {
        public int Time { set; get; }
        public int Data { set; get; }

        public Test()
        {
            Time = 10;
            Data = 20;
        }
    }
    public partial class TestPageView : UserControl
    {
        public TestPageView()
        {
            InitializeComponent();
            this.DataContext = new TestPageViewModel();


            List<Test> testList = new List<Test>(10);
            testList.Add(new Test());
            //testList.Add(new Test());

            
            gridTotal.ItemsSource = testList;
            

        }

    }
}
