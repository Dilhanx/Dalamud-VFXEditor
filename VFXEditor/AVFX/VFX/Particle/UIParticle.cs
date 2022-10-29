using ImGuiNET;
using System.Collections.Generic;
using System.IO;
using VfxEditor.AVFXLib;
using VfxEditor.AVFXLib.Particle;

namespace VfxEditor.AVFX.VFX {
    public class UIParticle : UINode {
        public readonly AVFXParticle Particle;
        public readonly UINodeGroupSet NodeGroups;
        public readonly UICombo<ParticleType> Type;
        public readonly List<UIParticleUVSet> UVSets;

        public UIData Data;

        public readonly List<UIItem> Animation;
        public readonly UITextureColor1 TC1;
        public readonly UITextureColor2 TC2;
        public readonly UITextureColor2 TC3;
        public readonly UITextureColor2 TC4;
        public readonly UITextureNormal TN;
        public readonly UITextureDistortion TD;
        public readonly UITexturePalette TP;
        public readonly UITextureReflection TR;
        public readonly List<UIItem> Tex;
        public readonly UIItemSplitView<UIItem> AnimationSplit;
        public readonly UIItemSplitView<UIItem> TexSplit;
        public readonly UIUVSetSplitView UVSplit;
        public readonly UINodeGraphView NodeView;
        private readonly List<IUIBase> Parameters;

        public UIParticle( AVFXParticle particle, UINodeGroupSet nodeGroups, bool hasDependencies = false ) : base( UINodeGroup.ParticleColor, hasDependencies ) {
            Particle = particle;
            NodeGroups = nodeGroups;
            NodeView = new UINodeGraphView( this );

            Animation = new List<UIItem>();
            Tex = new List<UIItem>();
            UVSets = new List<UIParticleUVSet>();

            Type = new UICombo<ParticleType>( "Type", Particle.ParticleVariety, onChange: () => {
                Particle.SetType( Particle.ParticleVariety.GetValue() );
                SetType();
            } );
            Parameters = new List<IUIBase> {
                new UIInt( "Loop Start", Particle.LoopStart ),
                new UIInt( "Loop End", Particle.LoopEnd ),
                new UICheckbox( "Use Simple Animation", Particle.SimpleAnimEnable ),
                new UICombo<RotationDirectionBase>( "Rotation Direction Base", Particle.RotationDirectionBaseType ),
                new UICombo<RotationOrder>( "Rotation Compute Order", Particle.RotationOrderType ),
                new UICombo<CoordComputeOrder>( "Coord Compute Order", Particle.CoordComputeOrderType ),
                new UICombo<DrawMode>( "Draw Mode", Particle.DrawModeType ),
                new UICombo<CullingType>( "Culling Type", Particle.CullingTypeType ),
                new UICombo<EnvLight>( "Enviornmental Light", Particle.EnvLightType ),
                new UICombo<DirLight>( "Directional Light", Particle.DirLightType ),
                new UICombo<UVPrecision>( "UV Precision", Particle.UvPrecisionType ),
                new UIInt( "Draw Priority", Particle.DrawPriority ),
                new UICheckbox( "Depth Test", Particle.IsDepthTest ),
                new UICheckbox( "Depth Write", Particle.IsDepthWrite ),
                new UICheckbox( "Soft Particle", Particle.IsSoftParticle ),
                new UIInt( "Collision Type", Particle.CollisionType ),
                new UICheckbox( "BS11", Particle.Bs11 ),
                new UICheckbox( "Apply Tone Map", Particle.IsApplyToneMap ),
                new UICheckbox( "Apply Fog", Particle.IsApplyFog ),
                new UICheckbox( "Enable Clip Near", Particle.ClipNearEnable ),
                new UICheckbox( "Enable Clip Far", Particle.ClipFarEnable ),
                new UIFloat2( "Clip Near", Particle.ClipNearStart, Particle.ClipNearEnd ),
                new UIFloat2( "Clip Far", Particle.ClipFarStart, Particle.ClipFarEnd ),
                new UICombo<ClipBasePoint>( "Clip Base Point", Particle.ClipBasePointType ),
                new UIInt( "Apply Rate Environment", Particle.ApplyRateEnvironment ),
                new UIInt( "Apply Rate Directional", Particle.ApplyRateDirectional ),
                new UIInt( "Apply Rate Light Buffer", Particle.ApplyRateLightBuffer ),
                new UICheckbox( "DOTy", Particle.DOTy ),
                new UIFloat( "Depth Offset", Particle.DepthOffset )
            };

            Animation.Add( new UILife( Particle.Life ) );
            Animation.Add( new UIParticleSimple( Particle.Simple, this ) );
            Animation.Add( new UICurve( Particle.Gravity, "Gravity" ) );
            Animation.Add( new UICurve( Particle.GravityRandom, "Gravity Random" ) );
            Animation.Add( new UICurve( Particle.AirResistance, "Air Resistance", locked: true ) );
            Animation.Add( new UICurve( Particle.AirResistanceRandom, "Air Resistance Random" ) );
            Animation.Add( new UICurve3Axis( Particle.Scale, "Scale", locked: true ) );
            Animation.Add( new UICurve3Axis( Particle.Rotation, "Rotation", locked: true ) );
            Animation.Add( new UICurve3Axis( Particle.Position, "Position", locked: true ) );
            Animation.Add( new UICurve( Particle.RotVelX, "Rotation Velocity X" ) );
            Animation.Add( new UICurve( Particle.RotVelY, "Rotation Velocity Y" ) );
            Animation.Add( new UICurve( Particle.RotVelZ, "Rotation Velocity Z" ) );
            Animation.Add( new UICurve( Particle.RotVelXRandom, "Rotation Velocity X Random" ) );
            Animation.Add( new UICurve( Particle.RotVelYRandom, "Rotation Velocity Y Random" ) );
            Animation.Add( new UICurve( Particle.RotVelZRandom, "Rotation Velocity Z Random" ) );
            Animation.Add( new UICurveColor( Particle.Color, "Color", locked: true ) );

            foreach( var uvSet in Particle.UVSets ) {
                UVSets.Add( new UIParticleUVSet( uvSet, this ) );
            }

            SetType();

            Tex.Add( TC1 = new UITextureColor1( Particle.TC1, this ) );
            Tex.Add( TC2 = new UITextureColor2( Particle.TC2, "Texture Color 2", this ) );
            Tex.Add( TC3 = new UITextureColor2( Particle.TC3, "Texture Color 3", this ) );
            Tex.Add( TC4 = new UITextureColor2( Particle.TC4, "Texture Color 4", this ) );
            Tex.Add( TN = new UITextureNormal( Particle.TN, this ) );
            Tex.Add( TR = new UITextureReflection( Particle.TR, this ) );
            Tex.Add( TD = new UITextureDistortion( Particle.TD, this ) );
            Tex.Add( TP = new UITexturePalette( Particle.TP, this ) );

            AnimationSplit = new UIItemSplitView<UIItem>( Animation );
            TexSplit = new UIItemSplitView<UIItem>( Tex );
            UVSplit = new UIUVSetSplitView( UVSets, this );
            HasDependencies = false; // if imported, all set now
        }

        public void SetType() {
            Data?.Dispose();
            Data = Particle.ParticleVariety.GetValue() switch {
                ParticleType.Model => new UIParticleDataModel( ( AVFXParticleDataModel )Particle.Data, this ),
                ParticleType.LightModel => new UIParticleDataLightModel( ( AVFXParticleDataLightModel )Particle.Data, this ),
                ParticleType.Powder => new UIParticleDataPowder( ( AVFXParticleDataPowder )Particle.Data ),
                ParticleType.Decal => new UIParticleDataDecal( ( AVFXParticleDataDecal )Particle.Data ),
                ParticleType.DecalRing => new UIParticleDataDecalRing( ( AVFXParticleDataDecalRing )Particle.Data ),
                ParticleType.Disc => new UIParticleDataDisc( ( AVFXParticleDataDisc )Particle.Data ),
                ParticleType.Laser => new UIParticleDataLaser( ( AVFXParticleDataLaser )Particle.Data ),
                ParticleType.Polygon => new UIParticleDataPolygon( ( AVFXParticleDataPolygon )Particle.Data ),
                ParticleType.Polyline => new UIParticleDataPolyline( ( AVFXParticleDataPolyline )Particle.Data ),
                ParticleType.Windmill => new UIParticleDataWindmill( ( AVFXParticleDataWindmill )Particle.Data ),
                ParticleType.Line => new UIParticleDataLine( ( AVFXParticleDataLine )Particle.Data ),
                _ => null
            };
        }

        public override void DrawInline( string parentId ) {
            var id = parentId + "/Ptcl";
            DrawRename( id );
            Type.DrawInline( id );
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            if( ImGui.BeginTabBar( id + "/Tabs", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton ) ) {
                if( ImGui.BeginTabItem( "Parameters" + id ) ) {
                    DrawParameters( id + "/Param" );
                    ImGui.EndTabItem();
                }
                if( Data != null && ImGui.BeginTabItem( "Data" + id ) ) {
                    DrawData( id + "/Data" );
                    ImGui.EndTabItem();
                }
                if( ImGui.BeginTabItem( "Animation" + id ) ) {
                    AnimationSplit.DrawInline( id + "/Animation" );
                    ImGui.EndTabItem();
                }
                if( ImGui.BeginTabItem( "UV Sets" + id ) ) {
                    UVSplit.DrawInline( id + "/UVSets" );
                    ImGui.EndTabItem();
                }
                if( ImGui.BeginTabItem( "Textures" + id ) ) {
                    TexSplit.DrawInline( id + "/Tex" );
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
        }

        private void DrawParameters( string id ) {
            ImGui.BeginChild( id );
            NodeView.DrawInline( id );
            IUIBase.DrawList( Parameters, id );
            ImGui.EndChild();
        }

        private void DrawData( string id ) {
            ImGui.BeginChild( id );
            Data.DrawInline( id );
            ImGui.EndChild();
        }

        public override string GetDefaultText() => $"Particle {Idx}({Particle.ParticleVariety.GetValue()})";

        public override string GetWorkspaceId() => $"Ptcl{Idx}";

        public override void Write( BinaryWriter writer ) => Particle.Write( writer );
    }
}
