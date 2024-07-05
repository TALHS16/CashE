using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddCategoryPopUp : MonoBehaviour
{
    public AddBTN animationBTN;

    public GameObject white_screen;

    public GameObject place;

    public TMP_InputField cat_name;
    public Sprite sprite;

    public Color color = default(Color);

    public GameObject cat_image;
    public GameObject place_holder_image;

    private ImagePicker picker = new ImagePicker();

    private bool open;

    public GameObject error_MSG;
    public TMP_Text error_contant;

    public TMP_Text title;

    private Renderer r;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
        r.sharedMaterial = r.material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenaddCategoryWindow()
    {
        open = true;
        animationBTN.SwitchState(true);
        SetActiveScreen();
        iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(1, 1, 1), "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.MoveTo(gameObject, iTween.Hash("position", white_screen.transform.position, "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.ColorTo(white_screen, iTween.Hash("a", 0.8, "time", 0.7f, "easetype", "easeOutCubic"));

    }

    public void PickImage()
    {
        picker.PickImage(512);
        cat_image.SetActive(true);
        cat_image.GetComponent<Image>().sprite = picker.sprite;
        place_holder_image.SetActive(false);
    }

    public void CloseWindow()
    {
        picker.sprite = null;
        open = false;
        CloseError();
        iTween.ColorTo(white_screen, iTween.Hash("a", 0, "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(0, 0, 0), "time", 0.7f, "easetype", "easeOutCubic"));
        iTween.MoveTo(gameObject, iTween.Hash("position", place.transform.position, "time", 0.7f, "easetype", "easeOutCubic", "oncomplete", "SetActiveScreen",
    "oncompletetarget", gameObject));
    }

    public void CloseError()
    {
        error_MSG.SetActive(false);
    }

    private void SetActiveScreen()
    {
        white_screen.SetActive(open);
    }

    public void PickColor()
    {
        ColorPicker.Create(r.sharedMaterial.color, "Choose Category Color", SetColor, ColorFinished, true);
    }

    private void SetColor(Color curr)
    {
        color = curr;
        title.color = color;
    }

    private void ColorFinished(Color end)
    {
        color = end;
    }

    public void CreateCategory()
    {
        sprite = picker.sprite;
        if (CheckFields())
        {
            CategoryModel category = new CategoryModel(cat_name.text, color);
            CategoryManager.Instance.AddCategory(category, sprite, this.gameObject);
            CloseWindow();
        }

    }

    public bool CheckFields()
    {
        bool flag = true;
        if (cat_name.text == "")
        {
            flag = false;
            error_MSG.SetActive(true);
            error_contant.text = "שכחת לבחור שם לקטגוריה";
        }
        else if (sprite == null)
        {
            flag = false;
            error_MSG.SetActive(true);
            error_contant.text = "אנא בחר/י אייקון לקטגוריה";
        }
        else if (color == default(Color))
        {
            flag = false;
            error_MSG.SetActive(true);
            error_contant.text = "אנא בחר/י צבע לקטגוריה";
        }
        else if (CategoryManager.Instance.CategoryDic.ContainsKey(cat_name.text))
        {
            flag = false;
            error_MSG.SetActive(true);
            error_contant.text = "הקטגוריה קיימת, אנא שים לב ובחר שם ייחודי";
        }
        else if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            flag = false;
            error_MSG.SetActive(true);
            error_contant.text = "חיבור אינטרנט אינו זמין כעת";
        }
        return flag;
    }

}
