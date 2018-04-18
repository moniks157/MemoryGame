using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh scoreLabel;
    [SerializeField] private TextMesh messageLabel;
    [SerializeField] private GameObject messageLabel2;
    public const int gridRows = 4;
    public const int gridCols = 4;
    public const float offsetX = 2f;
    public const float offsetY = 2f;

    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;

    private int _all = 0;
    private int _score = 0;

    public bool canReveal
    {
        get { return _secondRevealed == null; }
    }

    void Start()
    {
        Vector3 startPos = originalCard.transform.position;

        int[] numbers = new int[images.Length*2];
        for(int i=0;i<images.Length*2;i++)
        {
            numbers[i] = i / 2;
        }
        numbers = ShuffleArray(numbers);
        for (int i = 0; i<gridCols; i++) {
            for (int j = 0; j<gridRows; j++) {
                MemoryCard card;
                if (i == 0 && j == 0) {
                    card = originalCard;
                } else {
                    card = Instantiate(originalCard) as MemoryCard;
                }
                int index = j * gridCols + i;
                int id = numbers[index];
                card.SetCard(id, images[id]);
                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
    }

    public void CardRevealed(MemoryCard card)
    {
        if (_firstRevealed == null)
        {
            _firstRevealed = card;
        }
        else
        {
            _secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        if (_firstRevealed.id == _secondRevealed.id)
        {
            _score+=5;
            _all++;
            scoreLabel.text = _score.ToString();
            if(_all == images.Length)
            {
                messageLabel.text = "NICE";
                messageLabel2.SetActive(true);
            }
            yield return null;
        }
        else
        {
            _score--;
            scoreLabel.text = _score.ToString();
            yield return new WaitForSeconds(.8f);
            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
        }
        
        _firstRevealed = null;
        _secondRevealed = null;
    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    public void Restart()
    {
        messageLabel.text = "";
        messageLabel2.SetActive(false);
        SceneManager.LoadScene("MainScene");
    }
}