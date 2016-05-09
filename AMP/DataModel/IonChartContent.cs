using Anfema.Ion.Utils;
using Newtonsoft.Json;


namespace Anfema.Ion.DataModel
{
    public class IonChartContent : IonContent
    {
        [JsonProperty( "svg_data" )]
        public string svgData { get; set; }

        [JsonProperty( "chart_data" )]
        public string chartData { get; set; }

        [JsonProperty( "chart_config" )]
        public string chartConfig { get; set; }


        /// <summary>
        /// Checks the given IonChartContent for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if both ChartContents are equal</returns>
        public override bool Equals( object obj )
        {
            // Basic IonContent equality check
            if( !base.Equals( obj ) )
            {
                return false;
            }

            try
            {
                // Try to cast to chartContent and check for equality
                IonChartContent content = (IonChartContent)obj;

                return svgData.Equals( content.svgData )
                    && chartData.Equals( content.chartData )
                    && chartConfig.Equals( content.chartConfig );
            }

            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Returns the hashCode calculated for the elements values
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return EqualsUtils.calcHashCode( svgData, chartData, chartConfig );
        }
    }
}
