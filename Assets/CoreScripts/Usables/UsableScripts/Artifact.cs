using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

[CreateAssetMenu(fileName = "New Artifact Object", menuName = "Usables/Artifacts/Empty")]
public class Artifact : Usable
{
    // DATA //


    // OVERRIDES //
    // Basic Functions
    protected override void Awake()
    {
        itemType = UsableType.Artifact;
    }


    // FUNCTIONS //
    public void DiscoverArtifact()
    {
        if (ArtifactDataList.AddNewArtifact(this))
        {
            // Runs an animation
            if(DataRef.overlayManagerReference != null)
            {
                StringBuilder notificationBuilder = new StringBuilder();
                notificationBuilder.Append("You have discovered a new ");
                notificationBuilder.Append("<b>" + GameUtility.GenerateHTMLColouredText("Artifact", Color.magenta) + "</b>");
                notificationBuilder.Append("!");

                DataRef.overlayManagerReference.DisplayNotification(notificationBuilder.ToString());
            }
        }
    }
}
