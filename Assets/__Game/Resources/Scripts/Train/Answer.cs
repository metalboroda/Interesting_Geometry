using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.__Game.Resources.Scripts.Train
{
  public class Answer : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
  {
    [SerializeField] public Image _image;

    public Sprite AnswerSprite { get; private set; }

    private Vector3 _initLocalPosition;
    private Vector3 _offset;
    private bool _placed = false;

    private Camera _mainCamera;
    private BoxCollider _boxCollider;

    void Awake()
    {
      _mainCamera = Camera.main;
      _boxCollider = GetComponent<BoxCollider>();

      _initLocalPosition = transform.localPosition;
    }

    private void Start()
    {
      SetSpriteAndImage(_image.sprite);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.TryGetComponent(out Variant.Variant variant))
      {
        if (variant.ShowSprite == false)
        {
          variant.Place(this);

          _placed = true;
        }
      }
    }

    public void DisableVisual()
    {
      _boxCollider.enabled = false;

      _image.gameObject.SetActive(false);
    }

    public void SetSpriteAndImage(Sprite sprite)
    {
      AnswerSprite = sprite;
      _image.sprite = AnswerSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      _offset = transform.position - _mainCamera.ScreenToWorldPoint(
        new Vector3(eventData.position.x, eventData.position.y, transform.position.z));
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (_placed == true) return;

      Vector3 newPosition = new Vector3(eventData.position.x, eventData.position.y, transform.position.z);

      transform.position = _mainCamera.ScreenToWorldPoint(newPosition) + _offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      if (_placed == true) return;

      transform.DOLocalMove(_initLocalPosition, 0.25f);
    }
  }
}