using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class TransactionManager : MonoBehaviour
{
    public List<TransactionModel> transactions = new List<TransactionModel>();
    private PieChart pie;

    private MonthChooser monthChooser;
    private int month_curr;
    private int year_curr;

    private DetailList detailList;

    int curr_index;

    public ImageManager imageManager;
    public ImageStorage imageStorage;

    private static TransactionManager instance;
    // Start is called before the first frame update

    public static TransactionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TransactionManager>();
                instance.imageManager = new ImageManager("usageQueueImages.txt", "images");
                instance.imageStorage = new ImageStorage("images");
            }
            return instance;
        }
    }

    public void Filter(HashSet<string> cat, HashSet<string> people, DateTime? start_select, DateTime? end_select, BarChart bar_chart)
    {
        List<TransactionModel> result = new List<TransactionModel>();
        foreach (TransactionModel item in transactions)
        {
            DateTime curr = ConvertTimeStampToDataTime(item.timestamp);
            if (cat.Contains(item.category) && people.Contains(item.name) && curr >= start_select && curr <= end_select)
            {
                result.Add(item);
            }
        }
        if (end_select.HasValue && start_select.HasValue)
        {
            bar_chart.LatestTimestamp = ConvertDateTimeToTimestamp(end_select.Value);
            bar_chart.EarliestTimestamp = ConvertDateTimeToTimestamp(start_select.Value);
        }
        bar_chart.Transactions = result;
        bar_chart.BuildBarChart();
    }

    public long ConvertDateTimeToTimestamp(DateTime time)
    {
        DateTimeOffset dateTimeOffset = new DateTimeOffset(time);
        return dateTimeOffset.ToUnixTimeSeconds();
    }

    public PieChart Pie
    {
        get { return pie; }
        set
        {
            pie = value;
            RebuildPieChart(pie.type);
        }
    }
    public MonthChooser Month
    {
        get { return monthChooser; }
        set
        {
            monthChooser = value;
            HashSet<KeyValuePair<int, int>> get_month;
            if (pie)
            {
                get_month = GetUniqueMonths(pie.type);
                monthChooser.setValues(get_month, this, pie.type);
            }
            else
            {
                get_month = GetUniqueMonths(TransactionType.EXPENSE);
                monthChooser.setValues(get_month, this, TransactionType.EXPENSE);
            }

        }
    }

    public DetailList Details
    {
        get { return detailList; }
        set { detailList = value; }

    }

    void Awake()
    {
        FirebaseManager.Instance.GetAllTransactions(TransactionManager.Instance, null, TransactionType.EXPENSE, null);

    }

    public DateTime ConvertTimeStampToDataTime(long timestamp)
    {
        DateTime date_time = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
        return TimeZoneInfo.ConvertTime(date_time, TimeZoneInfo.Utc, TimeZoneInfo.Local);
    }

    HashSet<KeyValuePair<int, int>> GetUniqueMonths(TransactionType type)
    {
        HashSet<KeyValuePair<int, int>> uniqueMonths = new HashSet<KeyValuePair<int, int>>();

        // Extract and store unique months from the list of transactions
        foreach (TransactionModel trans in transactions)
        {
            DateTime date = ConvertTimeStampToDataTime(trans.timestamp);
            if (trans.type == type)
            {
                uniqueMonths.Add(new KeyValuePair<int, int>(date.Month, date.Year));
                month_curr = date.Month;
                year_curr = date.Year;
            }
        }
        return uniqueMonths;
    }

    public HashSet<string> GetUniqueYears()
    {
        HashSet<string> uniqueYears = new HashSet<string>();

        // Extract and store unique months from the list of transactions
        foreach (TransactionModel trans in transactions)
        {
            DateTime date = ConvertTimeStampToDataTime(trans.timestamp);
            uniqueYears.Add((date.Year).ToString());
        }
        return uniqueYears;
    }

    public List<TransactionModel> GetExpansesByMonth(int month, int year, TransactionType type)
    {
        List<TransactionModel> trans_list = new List<TransactionModel>();
        foreach (TransactionModel trans in transactions)
        {
            if (trans.type == type)
            {
                if (month == ConvertTimeStampToDataTime(trans.timestamp).Month && year == ConvertTimeStampToDataTime(trans.timestamp).Year)
                {
                    trans_list.Add(trans);
                }
            }
        }
        return trans_list;
    }

    public Dictionary<string, string> GetFirstWordDict()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        foreach (TransactionModel trans in transactions)
        {
            string first_word = (trans.description.Split())[0];
            if (!dic.ContainsKey(first_word))
            {
                dic.Add(first_word, trans.category);
            }
        }
        return dic;
    }

    public Dictionary<string, float> GetExpanseByCatagory(List<TransactionModel> trans_list, TransactionType type)
    {
        Dictionary<string, float> category_dic = new Dictionary<string, float>();
        foreach (TransactionModel trans in trans_list)
        {
            if (trans.type == type)
            {
                if (!category_dic.ContainsKey(trans.category))
                {
                    category_dic.Add(trans.category, trans.amount);
                }
                else
                {
                    category_dic[trans.category] += trans.amount;
                }
            }
        }
        return category_dic;
    }

    List<TransactionModel> GetAllExpanseByCatagory(List<TransactionModel> trans_list, string catagory, TransactionType type)
    {
        List<TransactionModel> trans_cat_list = new List<TransactionModel>();
        foreach (TransactionModel trans in trans_list)
        {
            if (trans.type == type)
            {
                if (catagory == trans.category)
                {
                    trans_cat_list.Add(trans);
                }
            }
        }
        return trans_cat_list;
    }

    public void BuildDetailList(string catagory, TransactionType type)
    {
        List<TransactionModel> month_list = GetExpansesByMonth(month_curr, year_curr, type);
        List<TransactionModel> category_list = GetAllExpanseByCatagory(month_list, catagory, type);
        detailList.clear();
        detailList.setValues(category_list, CategoryManager.Instance.CategoryDic[catagory]);

    }

    public void BuildPieChart(int month, int year, TransactionType type)
    {
        month_curr = month;
        year_curr = year;
        List<TransactionModel> month_list = GetExpansesByMonth(month, year, type);
        Dictionary<string, float> category_dic = GetExpanseByCatagory(month_list, type);
        pie.clear();
        pie.setValues(category_dic);

    }

    public void RebuildPieChart(TransactionType type = TransactionType.EXPENSE)
    {
        BuildPieChart(month_curr, year_curr, type);
    }

    public void AddTransaction(TransactionModel transactionModel, TransactionType type, PopUpWindow popUp)
    {
        FirebaseManager.Instance.GetAllTransactions(this, transactionModel, type, popUp);
    }

    public void editTransaction(TransactionModel transactionModel, TransactionModel old_model, PopUpWindow popUp)
    {
        TargetManager.Instance.EditCategory(transactionModel, old_model);
        byte[] byte_jpg = null;
        if (popUp.sprite != null)
        {
            transactionModel.has_image = true;
            byte_jpg = Resize(popUp.sprite.texture, 465, 620);

        }
        else
        {
            transactionModel.has_image = false;
        }
        FirebaseManager.Instance.SendTransactionUpdateToDatabase(transactionModel, old_model.id.ToString(), byte_jpg, popUp.gameObject);
    }

    public void SwitchIncomeOutcome(TransactionType type)
    {
        foreach (Transform child in monthChooser.transform)
        {
            Destroy(child.gameObject);
        }
        HashSet<KeyValuePair<int, int>> get_month = GetUniqueMonths(type);
        monthChooser.setValues(get_month, this, type);
        BuildPieChart(month_curr, year_curr, type);
    }

    Texture2D duplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    private byte[] Resize(Texture2D texture, int newWidth, int newHeight)
    {
        Texture2D readableTexture = duplicateTexture(texture);

        for (int w = 0; w < readableTexture.width; w++)
        {
            for (int h = 0; h < readableTexture.height; h++)
            {
                Color pixel = readableTexture.GetPixel(w, h);
                float grey = pixel.grayscale;
                readableTexture.SetPixel(w, h, new Color(grey, grey, grey));
            }
        }
        readableTexture.Apply();

        RenderTexture rt = new RenderTexture(newWidth, newHeight, 24);
        RenderTexture.active = rt;
        Graphics.Blit(readableTexture, rt);
        Texture2D result = new Texture2D(newWidth, newHeight);
        result.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        result.Apply();
        // readableTexture.Reinitialize(newWidth, newHeight, TextureFormat.RGB24, false);
        // readableTexture.Apply();

        byte[] jpgBytes = result.EncodeToJPG();

        return jpgBytes;
    }




    public void SetList(TransactionsList list, TransactionModel transactionModel, TransactionType type, PopUpWindow popUp)
    {
        transactions = list.transactions;
        if (transactionModel != null)
        {
            curr_index = -1;
            foreach (TransactionModel item in transactions)
            {
                if (curr_index < item.id)
                    curr_index = item.id;
            }
            curr_index += 1;
            transactionModel.id = curr_index;
            if (TargetManager.Instance.TargetDic.ContainsKey(transactionModel.category) && transactionModel.type == TransactionType.EXPENSE)
            {
                TargetModel target = TargetManager.Instance.TargetDic[transactionModel.category];
                DateTime timestamp_end = DateTime.Now;
                DateTime timestamp_begin = DateTime.Now;
                DateTime current = ConvertTimeStampToDataTime(transactionModel.timestamp);
                if (target.time_goal == 1)
                {
                    timestamp_end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    timestamp_end = timestamp_end.AddMonths(1);
                    timestamp_begin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                }
                else
                {
                    timestamp_end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    int daysToAdd = (int)timestamp_end.DayOfWeek - (int)DayOfWeek.Saturday;
                    // Subtract the days to get the first day of the week
                    timestamp_end = timestamp_end.AddDays(daysToAdd);

                    timestamp_begin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    int daysToSubtract = (int)timestamp_end.DayOfWeek - (int)DayOfWeek.Sunday;
                    // Subtract the days to get the first day of the week
                    timestamp_end = timestamp_end.AddDays(-daysToSubtract);
                }
                if (current >= timestamp_begin && current <= timestamp_end)
                {
                    target.current_amount += transactionModel.amount;
                    FirebaseManager.Instance.SendTargetUpdateCurrentAmountToDatabase(target);
                }
                else
                {
                    TargetHistoryManager.Instance.AddToFormerTarget(target, transactionModel);
                }
            }
            if (popUp.sprite != null)
            {
                transactionModel.has_image = true;
            }
            transactions.Add(transactionModel);
            transactions = transactions.OrderBy(t => t.timestamp).ToList();
            if (popUp.sprite != null)
            {
                byte[] byte_jpg = Resize(popUp.sprite.texture, 465, 620);
                FirebaseManager.Instance.UploadSprite(byte_jpg, curr_index.ToString(), "images/", ".jpg", popUp.gameObject, imageManager, imageStorage);

            }
            FirebaseManager.Instance.SendNewTransactionToDatabase(transactionModel, curr_index);
            foreach (Transform child in monthChooser.transform)
            {
                Destroy(child.gameObject);
            }
            HashSet<KeyValuePair<int, int>> get_months = GetUniqueMonths(TransactionType.EXPENSE);
            DateTime date = ConvertTimeStampToDataTime((transactionModel.timestamp));
            monthChooser.setValues(get_months, this, type);
            BuildPieChart(month_curr, year_curr, type);
        }
        else
        {
            SceneManager.LoadScene(2);
        }

    }

    public Tuple<string, float, string, float> GetStatDate()
    {
        Dictionary<string, float> dic_income = new Dictionary<string, float>();
        Dictionary<string, float> dic_outcome = new Dictionary<string, float>();
        foreach (TransactionModel trans in transactions)
        {
            string temp = ConvertTimeStampToDataTime(trans.timestamp).ToString("MM/yyyy");
            if (trans.type == TransactionType.EXPENSE)
            {
                if (!dic_income.ContainsKey(temp))
                {
                    dic_income.Add(temp, trans.amount);
                }
                else
                {
                    dic_income[temp] += trans.amount;
                }
                if (!dic_outcome.ContainsKey(temp))
                {
                    dic_outcome.Add(temp, -1 * trans.amount);
                }
                else
                {
                    dic_outcome[temp] -= trans.amount;
                }

            }
            else
            {
                if (!dic_income.ContainsKey(temp))
                {
                    dic_outcome.Add(temp, trans.amount);
                }
                else
                {
                    dic_outcome[temp] += trans.amount;
                }
            }
        }
        Tuple<string, float> income_tuple = GetMaxDate(dic_income);
        Tuple<string, float> outcome_tuple = GetMaxDate(dic_outcome);
        return new Tuple<string, float, string, float>(income_tuple.Item1, income_tuple.Item2, outcome_tuple.Item1, outcome_tuple.Item2);
    }

    public Tuple<string, float> GetMaxDate(Dictionary<string, float> dic)
    {
        float max_value = float.MinValue;
        string date = "";
        foreach (KeyValuePair<string, float> i in dic)
        {
            if (max_value < i.Value)
            {
                max_value = i.Value;
                date = i.Key;
            }
        }
        return new Tuple<string, float>(date, max_value);
    }

    public float GetMaxAmount(Dictionary<DateTime, float> dic)
    {
        float max_value = float.MinValue;
        foreach (KeyValuePair<DateTime, float> i in dic)
        {
            if (max_value < i.Value)
            {
                max_value = i.Value;
            }
        }
        return max_value;
    }
    public float GetMinAmount(Dictionary<DateTime, float> dic)
    {
        float max_value = float.MaxValue;
        foreach (KeyValuePair<DateTime, float> i in dic)
        {
            if (max_value > i.Value)
            {
                max_value = i.Value;
            }
        }
        return max_value;
    }

    public string GetWastfulCat()
    {
        Dictionary<string, float> dic = GetExpanseByCatagory(transactions, TransactionType.EXPENSE);
        float max_value = float.MinValue;
        string cat = "";
        foreach (KeyValuePair<string, float> i in dic)
        {
            if (max_value < i.Value)
            {
                max_value = i.Value;
                cat = i.Key;
            }
        }
        return cat;
    }

    public List<TransactionModel> GetAllTransactionUntilDate(DateTime end_date)
    {
        List<TransactionModel> result = new List<TransactionModel>();
        foreach (TransactionModel item in transactions)
        {
            DateTime curr = ConvertTimeStampToDataTime(item.timestamp);
            if (curr <= end_date)
            {
                result.Add(item);
            }
        }
        return result;
    }

    public Dictionary<string, List<TransactionModel>> GetAllTransactionsByUser()
    {
        Dictionary<string, List<TransactionModel>> dic = new Dictionary<string, List<TransactionModel>>();
        foreach (TransactionModel item in transactions)
        {
            if (!dic.ContainsKey(item.name))
            {
                dic.Add(item.name, new List<TransactionModel>());
            }
            (dic[item.name]).Add(item);

        }
        return dic;

    }

    public float GetAmountByTimePeriod(int time_goal, string category)
    {
        float sum = 0;
        DateTime from_date;
        switch (time_goal)
        {
            case 1:
                from_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                break;
            case 2:
                from_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                int daysToSubtract = (int)from_date.DayOfWeek - (int)DayOfWeek.Sunday;
                // Subtract the days to get the first day of the week
                from_date = from_date.AddDays(-daysToSubtract);
                break;
            default:
                from_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                break;
        }

        long from_timestamp = ConvertDateTimeToTimestamp(from_date);

        foreach (TransactionModel item in transactions)
        {
            if (item.timestamp >= from_timestamp && item.category == category)
            {
                sum += item.amount;
            }
        }

        return sum;
    }

    public Dictionary<DateTime, float> GetTransactionsAmountByMonth(List<TransactionModel> list, bool profit)
    {
        Dictionary<DateTime, float> dic = new Dictionary<DateTime, float>();
        foreach (TransactionModel item in list)
        {
            DateTime temp_date = ConvertTimeStampToDataTime(item.timestamp);
            temp_date = new DateTime(temp_date.Year, temp_date.Month, 1);
            if (profit)
            {
                if (item.type == TransactionType.INCOME)
                {
                    if (!dic.ContainsKey(temp_date))
                    {
                        dic.Add(temp_date, item.amount);
                    }
                    else
                    {
                        dic[temp_date] += item.amount;
                    }
                }
                else
                {
                    if (!dic.ContainsKey(temp_date))
                    {
                        dic.Add(temp_date, -1 * item.amount);
                    }
                    else
                    {
                        dic[temp_date] -= item.amount;
                    }
                }

            }
            else
            {
                if (!dic.ContainsKey(temp_date))
                {
                    dic.Add(temp_date, item.amount);
                }
                else
                {
                    dic[temp_date] += item.amount;
                }
            }

        }
        return dic;
    }
}
