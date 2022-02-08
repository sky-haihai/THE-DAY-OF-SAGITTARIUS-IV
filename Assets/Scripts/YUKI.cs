using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using XiheFramework;

public class YUKI : MonoBehaviour {
    public ForwardRendererData rendererData;

    private StencilStateData stencil;

    private void Start() {
        Game.Event.Subscribe("YUKI", OnYUKI);

        stencil = new StencilStateData {
            overrideStencilState = true,
            stencilReference = 0,
            stencilCompareFunction = CompareFunction.Always,
            passOperation = StencilOp.Keep,
            failOperation = StencilOp.Keep,
            zFailOperation = StencilOp.Keep
        };
    }

    private void OnYUKI(object sender, object e) {
        stencil.overrideStencilState = !stencil.overrideStencilState;
        rendererData.defaultStencilState = stencil;
    }
}