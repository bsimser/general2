using UnityEngine;
using UnityEngine.EventSystems;

namespace Devdog.General2.UI
{
    public class DraggableWindow : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Dragging")]
        public float dragSpeed = 1.0f;

        /// <summary>
        /// Once clicked should this draggable window be moved to the foreground?
        /// </summary>
        [Header("Bring to foreground")]
        public bool onClickBringToForeground = true;

        /// <summary>
        /// When the window is shown, should we bring this element to the foreground?
        /// </summary>
        public bool onWindowShowBringToForeground = true;

        /// <summary>
        /// The max sibling index this window can get when bringing it to the foreground.
        /// </summary>
        public int maxForegroundIndex = 10;

        [Required]
        public UIWindow windowToDrag;


        [Header("Cursor")]
        public CursorIcon dragCursor;
        private CursorIcon _cursorBefore;
//        public Texture2D cursor;
//        public Vector2 cursorHotspot;

//        public Texture2D cursorReset;
//        public Vector2 cursorHotspotReset;


        
        
        private Vector2 _dragOffset;
        private RectTransform _rectTransform;

        /// <summary>
        /// The window that is currently "focused" / on top.
        /// </summary>
        public static DraggableWindow currentWindow { get; protected set; }


        private UIWindow _window;
        public UIWindow window
        {
            get
            {
                if (_window == null)
                    _window = windowToDrag.GetComponent<UIWindow>();

                return _window;
            }
            set { _window = value; }
        }

        public void Awake()
        {
            _rectTransform = windowToDrag.GetComponent<RectTransform>();
            if (onWindowShowBringToForeground)
            {
                window.OnShow += MoveToForeground;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragOffset = new Vector2(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y) - eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition = new Vector3(eventData.position.x + _dragOffset.x * dragSpeed, eventData.position.y + _dragOffset.y * dragSpeed, 0.0f);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (onClickBringToForeground)
            {
                MoveToForeground();
            }
        }

        /// <summary>
        /// Move this draggable window back by 1.
        /// </summary>
        public virtual void MoveBack()
        {
            windowToDrag.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
        }

        /// <summary>
        /// Move this draggable window all the way back.
        /// </summary>
        public virtual void MoveToBackground()
        {
            windowToDrag.transform.SetAsFirstSibling();
        }

        /// <summary>
        /// Move this draggable window up by 1.
        /// </summary>
        public virtual void MoveUp()
        {
            windowToDrag.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
        }

        /// <summary>
        /// Bring this draggable window all the way to the front (maxSiblingIndex)
        /// </summary>
        public virtual void MoveToForeground()
        {
            if (currentWindow == this)
                return; // Already top window.

            windowToDrag.transform.SetSiblingIndex(maxForegroundIndex);
            currentWindow = this;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (dragCursor.texture != null)
            {
                dragCursor.Enable();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (dragCursor.texture != null)
            {
                GeneralSettingsManager.instance.settings.defaultCursor.Enable();
            }
        }
    }
}