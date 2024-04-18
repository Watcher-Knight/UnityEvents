using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class EventSettings : SettingsProvider
{
    private bool addTag = false;
    private static bool tagFoldout = false;
    private int tagEdit = -1;
    string newTagName = "NewTag";

    public EventSettings(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords) { }

    [SettingsProvider]
    public static SettingsProvider CreateSettingProvider() =>
        new EventSettings("Project/Event Settings", SettingsScope.Project);

    public override void OnGUI(string searchContext)
    {
        tagFoldout = EditorGUILayout.Foldout(tagFoldout, "Event Tags");

        if (tagFoldout)
        {
            int index = 0;
            foreach (EditorEventTag tag in EventData.GetTags())
            {
                GUILayout.BeginHorizontal();

                if (tagEdit == -1)
                {
                    EditorGUILayout.LabelField($"Tag #{index + 1}:", tag.Name);
                    if (GUILayout.Button("Edit"))
                    {
                        tagEdit = index;
                        newTagName = tag.Name;
                    }
                }
                else
                {
                    if (tagEdit == index)
                    {
                        EditorGUILayout.LabelField($"Tag #{index + 1}");
                        newTagName = EditorGUILayout.TextField(newTagName);
                        if (GUILayout.Button("Apply") || Event.current.keyCode == KeyCode.Return)
                        {
                            EventData.EditTag(index, newTagName);
                            tagEdit = -1;
                        }
                        if (GUILayout.Button("Cancel")) tagEdit = -1;
                        if (EventData.GetTags().Count() > 1 && GUILayout.Button("Delete"))
                        {
                            EventData.DeleteTag(tag.Name);
                            tagEdit = -1;
                        }
                        if (GUILayout.Button("Move Up"))
                        {
                            EventData.MoveTag(tag.Id, -1);
                            tagEdit -= 1;
                        }
                        if (GUILayout.Button("Move Down"))
                        {
                            EventData.MoveTag(tag.Id, 1);
                            tagEdit += 1;
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField($"Tag #{index + 1}:", tag.Name);
                    }
                }

                GUILayout.EndHorizontal();

                index += 1;
            }

            if (tagEdit == -1)
            {
                if (!addTag)
                {
                    if (GUILayout.Button("Add New")) addTag = true;
                }
                else
                {
                    newTagName = EditorGUILayout.TextField(newTagName);


                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("Add") || Event.current.keyCode == KeyCode.Return)
                    {
                        EventData.CreateTag(newTagName);
                        addTag = false;
                        newTagName = "NewTag";
                    }
                    if (GUILayout.Button("Cancel"))
                    {
                        addTag = false;
                        newTagName = "NewTag";
                    }

                    GUILayout.EndHorizontal();
                }
            }
        }
    }
}