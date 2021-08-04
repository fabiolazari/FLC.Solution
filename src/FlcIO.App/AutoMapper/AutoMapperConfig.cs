using AutoMapper;
using FlcIO.App.ViewModels;
using FlcIO.Business.Models;

namespace FlcIO.App.AutoMapper
{
	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig()
		{
			CreateMap<FlcMessage, FlcMessageViewModel>().ReverseMap();
		}
	}
}
