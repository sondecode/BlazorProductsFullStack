using Microsoft.AspNetCore.Components;

namespace BlazorProducts.Client.Component
{
	public partial class Davu
	{
		[Parameter]
		public string Title { get; set; }

		[Parameter(CaptureUnmatchedValues = true)]
		public Dictionary<string, object> AdditionalAttributes { get; set; }

		[CascadingParameter]
		public string Color1 { get; set; }

		[Parameter]
		public RenderFragment ChildContent { get; set; }
	}
}
