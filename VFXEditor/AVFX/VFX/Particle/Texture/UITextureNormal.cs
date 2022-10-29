using ImGuiNET;
using System.Collections.Generic;
using VfxEditor.AVFXLib;
using VfxEditor.AVFXLib.Particle;

namespace VfxEditor.AVFX.VFX {
    public class UITextureNormal : UIAssignableItem {
        public readonly AVFXParticleTextureNormal Tex;
        public readonly UIParticle Particle;
        public readonly string Name;

        public UINodeSelect<UITexture> TextureSelect;

        public readonly List<UIItem> Tabs;
        public readonly UIParameters Parameters;

        public UITextureNormal( AVFXParticleTextureNormal tex, UIParticle particle ) {
            Tex = tex;
            Particle = particle;

            Tabs = new List<UIItem> {
                ( Parameters = new UIParameters( "Parameters" ) )
            };

            if( IsAssigned() ) {
                Parameters.Add( TextureSelect = new UINodeSelect<UITexture>( Particle, "Texture", Particle.NodeGroups.Textures, Tex.TextureIdx ) );
            }

            Parameters.Add( new UICheckbox( "Enabled", Tex.Enabled ) );
            Parameters.Add( new UIInt( "UV Set Index", Tex.UvSetIdx ) );
            Parameters.Add( new UICombo<TextureFilterType>( "Texture Filter", Tex.TextureFilter ) );
            Parameters.Add( new UICombo<TextureBorderType>( "Texture Border U", Tex.TextureBorderU ) );
            Parameters.Add( new UICombo<TextureBorderType>( "Texture Border V", Tex.TextureBorderV ) );

            Tabs.Add( new UICurve( Tex.NPow, "Power" ) );
        }

        public override void DrawUnassigned( string parentId ) {
            if( ImGui.SmallButton( "+ Texture Normal" + parentId ) ) {
                AVFXBase.RecurseAssigned( Tex, true );

                Parameters.Remove( TextureSelect );
                Parameters.Prepend( TextureSelect = new UINodeSelect<UITexture>( Particle, "Texture", Particle.NodeGroups.Textures, Tex.TextureIdx ) );
            }
        }

        public override void DrawAssigned( string parentId ) {
            var id = parentId + "/TN";
            DrawListTabs( Tabs, id );
        }

        public override string GetDefaultText() => "Texture Normal";

        public override bool IsAssigned() => Tex.IsAssigned();
    }
}
