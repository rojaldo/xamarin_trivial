namespace AppTrivial
{
    internal class Card
    {
        string category;
        string type;
        string difficulty;
        public string question;
        public string correct_answer;
        dynamic[] incorrect_answers;
        public string[] answers;
        private dynamic result;
        public bool answered = false;

        public Card(dynamic card)
        {
            this.category = card.category;
            this.type = card.type;
            this.difficulty = card.difficulty;
            this.question = card.question;
            this.correct_answer = card.correct_answer;
            //this.incorrect_answers = card.incorrect_answers;
            this.answers = new string[card.incorrect_answers.Count + 1];
            for (int index = 0; index < card.incorrect_answers.Count; index++){
                this.answers[index] = card.incorrect_answers[index];
            }
            this.answers[this.answers.Length - 1] = this.correct_answer;
        }
    }
}