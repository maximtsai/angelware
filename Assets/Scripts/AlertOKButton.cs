using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertOKButton : MonoBehaviour
{
    public Alert alert;

    public void OKButtonClicked () {
        alert.BackToIdle();
    }
}
