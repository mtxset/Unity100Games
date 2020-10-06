using UnityEngine;
using UnityEngine.UI;

internal struct MenuEntry {
    public int Id;
    public string Value;
}

public class Menu : MonoBehaviour {

    public Camera CurrentCamera;
    public GameObject Canvas;
    public GameObject TextObject;
    public Text PagesText;
    public int EntriesPerPage;

    private string[] menuEntries;
    private GameObject[] textObjects;
    private int totalPages;
    private int currentPage = 0;

    public void NextPage() {
        if (currentPage + 1 < totalPages)
            currentPage++;

        updatePage();
    }

    public void PreviousPage() {
        if (currentPage - 1 >= 0) {
            currentPage--;
        }

        updatePage();
    }

    private void Start() {

        // Load game settings
        menuEntries = new string[10];

        textObjects = new GameObject[EntriesPerPage];

        for (int i = 0; i < menuEntries.Length; i++) {
            menuEntries[i] = $"Entry {i}";
        }

        createAllTextEntries();
    }  

    private void createAllTextEntries() {
        // offseting by 1 from both ends to stay in camera and leave space for pages
        var offset = (CurrentCamera.orthographicSize - 1) * 2 / (EntriesPerPage - 1);
        float lastPosition = CurrentCamera.orthographicSize - 0.5f;

        int i;
        for (i = 0; i < EntriesPerPage; i++) {
            var position = new Vector2(0, lastPosition);
            lastPosition -= offset;
            textObjects[i] = createText(position);
        }

        // show total pages
        totalPages = Mathf.CeilToInt((float)menuEntries.Length / (float)EntriesPerPage);
        updatePage();
    }

    private void updatePage() {
        // populate entries
        var startingPoint = currentPage * EntriesPerPage;

        // lazy reset
        for (int i = 0; i < EntriesPerPage; i++) {
            textObjects[i].GetComponent<Text>().text = "";
        }

        for (
            int menuIndex = startingPoint, textIndex = 0; 
            menuIndex < menuEntries.Length && textIndex < EntriesPerPage * (currentPage + 1); 
            menuIndex++, textIndex++) {
            textObjects[textIndex].GetComponent<Text>().text = menuEntries[menuIndex];
        }

        PagesText.text = $"Page: {currentPage + 1}; Total: {totalPages}";
    }

    private GameObject createText(Vector3 position) {
        var newEntry = Instantiate(TextObject, position, Quaternion.identity, Canvas.transform);

        newEntry.GetComponent<Text>().text = "123";
        newEntry.SetActive(true);
        
        return newEntry;
    }

    // 1. Array of entries
    // 2. Display all entries with pages
    // 3. On Action select entry and highlight it
    
}