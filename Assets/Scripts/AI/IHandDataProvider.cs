
public interface IHandDataProvider
{
    bool HasCardWithScore(int score);
    int GetHandScore();
    int GetCardCount();
    bool HasAceCardWithMaxScore();

}
