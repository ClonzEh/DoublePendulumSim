using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DoublePendulumSim
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var pendulum = new Pendulum(this.myCanvas) { m1 = 10, m2 = 10, Phi1 = 0*(Math.PI)/2, Phi2 = 2.3*(Math.PI)/2 };
            pendulum.Update();
            var timer = new System.Threading.Timer((state) => Dispatcher.Invoke(() => pendulum.Animate()), null, TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(5));
            var changed = new RoutedPropertyChangedEventHandler<double>((sender, e) =>
            {
                if (pendulum != null)
                {
                    lock(pendulum)
                    {
                        pendulum.m1 = mass1.Value;
                        pendulum.m2 = mass2.Value;
                        pendulum.Phi1 = Phi1.Value / 180 * Math.PI;
                        pendulum.Phi2 = Phi2.Value / 180 * Math.PI;
                        pendulum.Update();
                        timer.Change(TimeSpan.FromMilliseconds(250), TimeSpan.FromMilliseconds(5));
                    }
                }
            });
            mass1.ValueChanged += changed;
            mass2.ValueChanged += changed;
            Phi1.ValueChanged += changed;
            Phi2.ValueChanged += changed;
            this.Closed += (sender , e) => timer.Dispose();
        }
    }
}
