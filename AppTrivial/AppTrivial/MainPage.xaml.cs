using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppTrivial
{
    public partial class MainPage : ContentPage
    {

        int indexCard = 0;
        ObservableCollection<Card> cards= new ObservableCollection<Card>();
        private int points = 0;

        public MainPage()
        {
            InitializeComponent();
            getCards();
        }

        private async void getCards()
        {
            var data = await GetRequest("https://opentdb.com/api.php?amount=10");
            foreach(var item in data.results)
            {
                Card mycard = new Card(item);
                this.cards.Add(mycard);
            }
            this.updateCard();
        }

        private void updateCard()
        {
            label_question.Text = System.Net.WebUtility.HtmlDecode(cards[indexCard].question);
            while(myCard.Children.Count > 1)
            {
                myCard.Children.RemoveAt(1);
            }
            foreach (string answer in cards[indexCard].answers)
            {
                Button button = new Button { Text = answer, BackgroundColor = Color.FromRgb(72, 138, 255), TextColor = Color.White };
                button.Clicked += OnButtonClicked;
                myCard.Children.Add(button);

            } if(cards[indexCard].answered == true)
            {
                disableCard();
            }

        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            this.cards[indexCard].answered = true;
            disableCard();
            if (((Button)sender).Text != this.cards[indexCard].correct_answer)
            {
                this.points -= 1;
                ((Button)sender).BackgroundColor = Color.Red;
                ((Button)sender).TextColor = Color.White;
            } else
            {
                this.points += 2;
            }
            updatePoints();
        }

        private void updatePoints()
        {
            label_points.Text = this.points.ToString() + " puntos"; 
        }

        private void disableCard()
        {
            for (int index = 1; index < myCard.Children.Count; index++)
            {
                Button myButton = ((Button)myCard.Children[index]);
                myButton.IsEnabled = false;
                myButton.BackgroundColor = Color.FromRgb(100, 100, 100);
                myButton.TextColor = Color.White;
                if (myButton.Text == cards[indexCard].correct_answer)
                {
                    myButton.BackgroundColor = Color.Green;
                    myButton.TextColor = Color.White;
                }
            }
        }

        private async Task<dynamic> GetRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            dynamic data = null;
            using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        Console.Out.WriteLine("Response Body: \r\n {0}", content);
                        data = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                        return data;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        private void Button_Next(object sender, EventArgs e)
        {
            this.indexCard += 1;
            if(this.indexCard == cards.Count - 1)
            {
                button_next.IsEnabled = false;
            }else
            {
                button_next.IsEnabled = true;
            }
            this.updateCard();
        }
        private void Button_Prev(object sender, EventArgs e)
        {
            this.indexCard -= 1;
            if (this.indexCard == 0)
            {
                button_prev.IsEnabled = false;
            }else
            {
                button_prev.IsEnabled = true;
            }
            this.updateCard();
        }

    }
}
