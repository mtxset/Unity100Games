using UnityEngine;
using UnityEngine.UI;

namespace Components.UnityComponents {
public class BasicMenu : MonoBehaviour {
    public Camera CurrentCamera;
    public GameObject Canvas;
    public GameObject TextObject;
    public Text PagesText;
    public int EntriesPerPage;
    public Color DefaultColor;
    public Color HoveredOnColor;
    public Color SelectedColor;

    public Menu Menu;

    private void Start() {
        var menuParams = new MenuParams {
            CurrentCamera = this.CurrentCamera,
            Canvas = this.Canvas,
            TextObject = this.TextObject,
            PagesText = this.PagesText,
            EntriesPerPage = this.EntriesPerPage,
            DefaultColor = this.DefaultColor,
            HoveredOnColor = this.HoveredOnColor,
            SelectedColor = this.SelectedColor
        };
        Menu = new Menu(menuParams);
    }
}
}