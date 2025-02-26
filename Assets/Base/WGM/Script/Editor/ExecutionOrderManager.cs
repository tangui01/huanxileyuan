using UnityEditor;

[InitializeOnLoad]
public class ExecutionOrderManager : Editor {
	
	static ExecutionOrderManager()
	{
		foreach(MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts()) {
            if(monoScript.name == "DealCommand") {
                SetOrder(monoScript, -1000);
            }
		}
	}

    static void SetOrder(MonoScript monoScript, int order)
    {
        if(MonoImporter.GetExecutionOrder(monoScript) != order) {
            MonoImporter.SetExecutionOrder(monoScript, order);
        }
    }
	
}
