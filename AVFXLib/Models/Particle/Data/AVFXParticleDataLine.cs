using AVFXLib.AVFX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models {
    public class AVFXParticleDataLine : AVFXParticleData {
        public LiteralInt LineCount = new( "LnCT" );
        public AVFXCurve Length = new( "Len" );
        public AVFXCurveColor ColorBegin = new( name: "ColB" );
        public AVFXCurveColor ColorEnd = new( name: "ColE" );
        private readonly List<Base> Attributes;

        public AVFXParticleDataLine() : base( "Data" ) {
            Attributes = new List<Base>( new Base[]{
                LineCount,
                Length,
                ColorBegin,
                ColorEnd
            } );
        }

        public override void Read( AVFXNode node ) {
            Assigned = true;
            ReadAVFX( Attributes, node );
        }

        public override void ToDefault() {
            Assigned = true;
        }

        public override AVFXNode ToAVFX() {
            var dataAvfx = new AVFXNode( "Data" );
            PutAVFX( dataAvfx, Attributes );
            return dataAvfx;
        }
    }
}
