namespace TrackX.Application.Commons.Bases.Request;

public class BaseFiltersItinerarioRequest : BasePaginationRequest
{
    public int? NumFilter { get; set; } = null;
    public string? TextFilter { get; set; } = null;
    public string? PolFilter { get; set; } = null;
    public string? PoeFilter { get; set; } = null;
    public string? ModalidadFilter { get; set; } = null;
    public string? TransporteFilter { get; set; } = null;
    public int? StateFilter { get; set; } = null;
    public string? StartDate { get; set; } = null;
    public string? EndDate { get; set; } = null;
    public bool? Download { get; set; } = false;
}