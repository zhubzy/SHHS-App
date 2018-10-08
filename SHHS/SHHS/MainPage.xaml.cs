using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SHHS
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {    
            InitializeComponent();



            int column = 0;
            int row = 1;
            
            for (int date =1; date<= 31; date++)
            {
                var label = new Label { Text= $"{date}", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White};
                Calendar.Children.Add(label,column,row);

                if (date % 7 == 0)
                {
                    column = 0;
                    row++;
                }
                else {

                    column++;

                }


            }

            //RelativeView.Children.Add(new Button { Text = "Announcement1", BorderColor = Color.White, IsEnabled = false, CornerRadius = 35, BorderWidth = 2 },
            //                                                                Constraint.RelativeToParent(parent=>parent.X+35),
            //                                                                Constraint.RelativeToParent(parent=>parent.Width*0.8),
            //                                                                Constraint.RelativeToParent(parent=>monthLabel.Y+350),
            //                                                                Constraint.RelativeToParent(parent=>monthLabel.Height*2));
            //RelativeView.Children.Add(new Button { Text = "Announcement2", BorderColor = Color.White, CornerRadius = 35, BorderWidth = 2, IsEnabled = false },
                                                                            //Constraint.RelativeToParent(parent => parent.X + 35),
                                                                            //Constraint.RelativeToParent(parent => parent.Width * 0.8),
                                                                            //Constraint.RelativeToParent(parent => monthLabel.Y + 500),
                                                                            //Constraint.RelativeToParent(parent => monthLabel.Height * 2));

        }
        
        }



    }

