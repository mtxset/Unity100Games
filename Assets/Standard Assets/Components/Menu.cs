using System;
using UnityEngine;
using UnityEngine.UI;

namespace Components {
public struct MenuParams {
    public Camera CurrentCamera;
    public GameObject Canvas;
    public GameObject TextObject;
    public Text PagesText;
    public int EntriesPerPage;
    public Color DefaultColor;
    public Color HoveredOnColor;
    public Color SelectedColor;
}

public class Menu {
    
    public event Action<string> OnSelected;

    private string[] menuEntries;
    
    private MenuParams menuParams;

    private GameObject[] textObjects;
    private int totalPages;
    private int currentPage;
    private int hoveredMenuItem;
    private int currentlyShownItemCount;
    private int selectedMenuItem = -1;


    public Menu(MenuParams menuParams) {
        this.menuParams = menuParams;
    }

    public void UpdateMenuEntries(string[] newMenuEntries) {
        menuEntries = newMenuEntries;

        init();
    }

    public void Select() {
        selectedMenuItem = hoveredMenuItem + (menuParams.EntriesPerPage * currentPage);
        OnSelected?.Invoke(menuEntries[selectedMenuItem]);
        redrawPage();
    }

    public void NextMenuItem() {
        if (hoveredMenuItem + 1 < currentlyShownItemCount)
            hoveredMenuItem++;

        redrawPage();
    }

    public void PreviousMenuItem() {
        if (hoveredMenuItem - 1 >= 0)
            hoveredMenuItem--;

        redrawPage();
    }

    public void NextPage() {
        if (currentPage + 1 < totalPages) {
            hoveredMenuItem = 0;
            currentPage++;
        }

        redrawPage();
    }

    public void PreviousPage() {
        if (currentPage - 1 >= 0) {
            hoveredMenuItem = 0;
            currentPage--;
        }

        redrawPage();
    }

    private void init() {

        textObjects = new GameObject[menuParams.EntriesPerPage];

        createAllTextEntries();

        // calculate total pages
        totalPages = Mathf.CeilToInt((float)menuEntries.Length / (float)menuParams.EntriesPerPage);
        redrawPage();
    }

    private void createAllTextEntries() {
        // offseting by 1 from both ends to stay in camera and leave space for pages
        var offset = (menuParams.CurrentCamera.orthographicSize - 1) * 2 / (menuParams.EntriesPerPage - 1);
        float lastPosition = menuParams.CurrentCamera.orthographicSize - 0.5f;

        int i;
        for (i = 0; i < menuParams.EntriesPerPage; i++) {
            var position = new Vector2(0, lastPosition);
            lastPosition -= offset;
            textObjects[i] = createText(position);
        }
    }

    private void redrawPage() {
        // populate entries
        var startingPoint = currentPage * menuParams.EntriesPerPage;

        // lazy reset
        for (int i = 0; i < menuParams.EntriesPerPage; i++) {
            textObjects[i].GetComponent<Text>().text = "";
            textObjects[i].GetComponent<Text>().color = menuParams.DefaultColor;
            textObjects[i].GetComponent<Text>().fontStyle = FontStyle.Normal;
        }

        int menuIndex = startingPoint, textIndex = 0;

        for (; 
            menuIndex < menuEntries.Length && textIndex < menuParams.EntriesPerPage * (currentPage + 1); 
            menuIndex++, textIndex++) {
            // change color to green current hover over
            if (textIndex == hoveredMenuItem) {
                textObjects[textIndex].GetComponent<Text>().color = menuParams.HoveredOnColor;
                textObjects[textIndex].GetComponent<Text>().fontStyle = FontStyle.Bold;
            }

            // highlight selection
            if (menuIndex == selectedMenuItem) {
                textObjects[textIndex].GetComponent<Text>().color = menuParams.SelectedColor;
                textObjects[textIndex].GetComponent<Text>().fontStyle = FontStyle.Bold;
            }

            textObjects[textIndex].GetComponent<Text>().text = menuEntries[menuIndex];
        }

        currentlyShownItemCount = textIndex;

        menuParams.PagesText.text = $"Page: {currentPage + 1} out of {totalPages}. Total entries: {menuEntries.Length}";
    }

    private GameObject createText(Vector3 position) {
        var newEntry = UnityEngine.Object.Instantiate(
            menuParams.TextObject, 
            position, 
            Quaternion.identity, 
            menuParams.Canvas.transform);

        newEntry.GetComponent<Text>().text = "undefined";
        newEntry.SetActive(true);
        
        return newEntry;
    }
    
}
}