using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    [SerializeField] List<Worker> workers;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < workers.Count; i++)
        {
            workers[i].SubscribeToOnDeath(TheWorkerIsDeadIKnewItExclamationMark);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void AreAllTheWorkersDeadYetQuestionMark()
    {
        if (workers.Count <= 0)
        {
            GameManager.GetGameManager().DisplayLossMessage();
        }
    }
    void TheWorkerIsDeadIKnewItExclamationMark(Worker worker)
    {
        workers.Remove(worker);
        AreAllTheWorkersDeadYetQuestionMark();
    }
}
