namespace DotCon;

public class ScriptManager
{
    protected readonly Dictionary<string, string[]> _actions = new();
    
    /// <summary>
    /// Stores a script with the specified key and list of actions.
    /// </summary>
    /// <param name="key">The key to associate with the script.</param>
    /// <param name="actions">The list of actions that make up the script.</param>
    public void StoreScript(string key, List<string> actions)
    {
        StoreScript(key, actions.ToArray());
    }
    
    /// <summary>
    /// Stores a script with the specified key and array of actions.
    /// </summary>
    /// <param name="key">The key to associate with the script.</param>
    /// <param name="actions">The array of actions that make up the script.</param>
    public void StoreScript(string key, params string[] actions)
    {
        _actions.Add(key, actions);
    }
    
    /// <summary>
    /// Retrieves all stored scripts as an array of key-value pairs.
    /// </summary>
    /// <returns>An array of key-value pairs representing the stored scripts.</returns>
    public KeyValuePair<string, string[]>[] GetScripts()
    {
        return _actions.ToArray();
    }
    
    /// <summary>
    /// Removes the script associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the script to remove.</param>
    public void RemoveScript(string key)
    {
        _actions.Remove(key);
    }
    
    /// <summary>
    /// Clears all stored scripts.
    /// </summary>
    public void ClearScripts()
    {
        _actions.Clear();
    }
    
    /// <summary>
    /// Updates the script associated with the specified key using the provided list of actions.
    /// </summary>
    /// <param name="key">The key of the script to update.</param>
    /// <param name="actions">The list of actions to replace the existing actions in the script.</param>
    public void UpdateScript(string key, List<string> actions)
    {
        UpdateScript(key, actions.ToArray());
    }
    
    /// <summary>
    /// Updates the script associated with the specified key using the provided array of actions.
    /// </summary>
    /// <param name="key">The key of the script to update.</param>
    /// <param name="actions">The array of actions to replace the existing actions in the script.</param>
    public void UpdateScript(string key, params string[] actions)
    {
        _actions[key] = actions;
    }
}