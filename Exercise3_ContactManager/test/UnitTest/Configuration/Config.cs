using Api.Profiles;
using AutoMapper;

namespace UnitTest.Configuration;

public static class Config
{
	public static Mapper Mapper { get; }

	static Config()
	{
		// Mapper
		Mapper = new Mapper(new MapperConfiguration(cfg =>
			cfg.AddProfile(new MappingProfile())));
	}
}