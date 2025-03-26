using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizResponses : MonoBehaviour
{
    [System.Serializable]
    public class Response
    {
        public Question Question;
        public int SelectedResponseIndex;
    }

    public List<Response> responses = new List<Response>();
}
