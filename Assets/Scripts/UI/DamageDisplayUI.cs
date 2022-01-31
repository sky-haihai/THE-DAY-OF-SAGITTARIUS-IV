using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XiheFramework;

public class DamageDisplayUI : UIBehaviour {
    public ComputeShader drawArrowShader;

    private List<Image> damageArrows;

    //private ComputeBuffer m_ComputeBuffer;


    private void UpdateDamageArrows() {
        var data = Game.Blackboard.GetData<DamageDisplayStruct[]>("DamageDisplayData");

        // const int stride = sizeof(float) * 6; //vector3 + vector3
        // m_ComputeBuffer = new ComputeBuffer(data.Length, stride);
        // m_ComputeBuffer.SetData(data);
        // drawArrowShader.SetBuffer(0, "buffer", m_ComputeBuffer);
        // drawArrowShader.Dispatch(0, Screen.width / 8, Screen.height / 8, 1);
    }

    private void PlaceImage(DamageDisplayStruct d) {
        var delta = d.to - d.from;
        var offset = Vector3.Cross(delta, Vector3.up).normalized; //right

        offset *= 0.1f;
        var start = d.from + offset;
        var end = d.to + offset;
    }
}