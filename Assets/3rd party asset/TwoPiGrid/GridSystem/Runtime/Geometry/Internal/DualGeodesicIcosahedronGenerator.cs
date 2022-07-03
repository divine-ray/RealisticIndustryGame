/**
 * Interface for each link in the  Chain of Responsibility (pattern)
 * for generating different types of dual geodesic icosahedra.
 * 
 * Created by Jazz Emma Prats Camps in October 2020.
 */

namespace TwoPiGrid.Geometry.Internal
{
    internal abstract class DualGeodesicIcosahedronGenerator
    {
        private DualGeodesicIcosahedronGenerator nextInChain;

        protected DualGeodesicIcosahedronGenerator(DualGeodesicIcosahedronGenerator nextInChain)
        {
            this.nextInChain = nextInChain;
        }

        public DualGeodesicIcosahedron Generate(float radius, int order)
        {
            if (AmResponsible(order))
                return DoGenerate(radius);
            else
                return nextInChain?.Generate(radius, order);
        }

        protected abstract bool AmResponsible(int order);

        protected abstract DualGeodesicIcosahedron DoGenerate(float radius);
    }
}
