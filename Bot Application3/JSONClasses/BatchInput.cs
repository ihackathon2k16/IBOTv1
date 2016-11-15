using System.Collections.Generic;

// Classes to store the input for the sentiment API call
public class BatchInput
{
    public List<DocumentInput> documents { get; set; }
}
public class DocumentInput
{
    public string language { get;set; }
    public string id { get; set; }
    public string text { get; set; }
}

// Classes to store the result from the sentiment analysis
public class BatchResult
{
    public List<DocumentResult> documents { get; set; }
}
public class DocumentResult
{
    public double score { get; set; }
    public string id { get; set; }
}

public class KeyPhraseResult
{
    public List<KeyDocument> documents { get; set; }
    
}
public class KeyDocument
{
    public List<string> keyPhrases { get; set; }
    public string id { get; set; }
}

public class LUISResult
{
    public string query { get; set; }
    public Intent topScoringIntent { get; set; }
    public Entity   [] entities {get;set;}

    public string toString()
    {
        return topScoringIntent.intent;
    }
}

public class Intent
{
    public string intent { get; set; }
    public float score { get; set; }

}

public class Entity
{
    public string entity { get; set; }
    public string type { get; set; }
    public int startIndex { get; set; }
    public int endIndex { get; set; }
    public float score { get; set; }
}

public class LINGINPUT {
    public string language { get; set; }
    public List<string> anaid { get; set; }
    public string query { get; set; }

}

public class LINGResp
{
    public string anaid { get; set; }
    public List<string> result { get; set; }
}