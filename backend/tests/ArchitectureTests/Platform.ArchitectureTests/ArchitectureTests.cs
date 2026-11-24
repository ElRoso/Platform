using System.Reflection;
using NetArchTest.Rules;
using Xunit;

public class ArchitectureTests
{
    private static readonly Assembly BuildingBlocksDomainAssembly = typeof(Platform.BuildingBlocks.Domain.AssemblyMarker).Assembly;
    private static readonly Assembly BuildingBlocksApplicationAssembly = typeof(Platform.BuildingBlocks.Application.AssemblyMarker).Assembly;
    private static readonly Assembly BuildingBlocksInfrastructureAssembly = typeof(Platform.BuildingBlocks.Infrastructure.AssemblyMarker).Assembly;
    private static readonly Assembly BuildingBlocksApiAssembly = typeof(Platform.BuildingBlocks.Api.AssemblyMarker).Assembly;

    private static readonly Assembly IdentityDomainAssembly = typeof(Platform.Identity.Domain.AssemblyMarker).Assembly;
    private static readonly Assembly IdentityApplicationAssembly = typeof(Platform.Identity.Application.AssemblyMarker).Assembly;
    private static readonly Assembly IdentityInfrastructureAssembly = typeof(Platform.Identity.Infrastructure.AssemblyMarker).Assembly;
    private static readonly Assembly IdentityApiAssembly = typeof(Platform.Identity.Api.AssemblyMarker).Assembly;
    private static readonly Assembly IdentityContractsAssembly = typeof(Platform.Identity.Contracts.AssemblyMarker).Assembly;

    [Fact]
    public void BuildingBlocksDomain_DoesNotDependOn_Application_Infrastructure_Api()
    {
        var result = Types.InAssembly(BuildingBlocksDomainAssembly)
            .Should().NotHaveDependencyOnAny(new[] { "Platform.BuildingBlocks.Application", "Platform.BuildingBlocks.Infrastructure", "Platform.BuildingBlocks.Api" })
            .GetResult();

        Assert.True(result.IsSuccessful, Format(result));
    }

    [Fact]
    public void BuildingBlocksApplication_DoesNotDependOn_Infrastructure_Api()
    {
        var result = Types.InAssembly(BuildingBlocksApplicationAssembly)
            .Should().NotHaveDependencyOnAny(new[] { "Platform.BuildingBlocks.Infrastructure", "Platform.BuildingBlocks.Api" })
            .GetResult();

        Assert.True(result.IsSuccessful, Format(result));
    }

    [Fact]
    public void BuildingBlocksApi_DoesNotDependOn_Infrastructure()
    {
        var result = Types.InAssembly(BuildingBlocksApiAssembly)
            .Should().NotHaveDependencyOnAny(new[] { "Platform.BuildingBlocks.Infrastructure" })
            .GetResult();

        Assert.True(result.IsSuccessful, Format(result));
    }

    [Fact]
    public void Domain_DoesNotDependOn_Infrastructure_Api_EfCore_AspNetCore()
    {
        foreach (var assembly in new[] { BuildingBlocksDomainAssembly, IdentityDomainAssembly })
        {
            var result = Types.InAssembly(assembly)
                .Should().NotHaveDependencyOnAny(new[] { "Microsoft.EntityFrameworkCore", "Microsoft.AspNetCore", "Platform.BuildingBlocks.Infrastructure", "Platform.BuildingBlocks.Api", "Platform.Identity.Infrastructure", "Platform.Identity.Api" })
                .GetResult();

            Assert.True(result.IsSuccessful, Format(result));
        }
    }

    [Fact]
    public void Application_DoesNotDependOn_Infrastructure_Api()
    {
        foreach (var assembly in new[] { BuildingBlocksApplicationAssembly, IdentityApplicationAssembly })
        {
            var result = Types.InAssembly(assembly)
                .Should().NotHaveDependencyOnAny(new[] { "Platform.BuildingBlocks.Infrastructure", "Platform.BuildingBlocks.Api", "Platform.Identity.Infrastructure", "Platform.Identity.Api" })
                .GetResult();

            Assert.True(result.IsSuccessful, Format(result));
        }
    }

    [Fact]
    public void Api_DoesNotDependOn_Infrastructure_EfCore()
    {
        foreach (var assembly in new[] { BuildingBlocksApiAssembly, IdentityApiAssembly })
        {
            var result = Types.InAssembly(assembly)
                .Should().NotHaveDependencyOnAny(new[] { "Platform.BuildingBlocks.Infrastructure", "Platform.Identity.Infrastructure", "Microsoft.EntityFrameworkCore" })
                .GetResult();

            Assert.True(result.IsSuccessful, Format(result));
        }
    }

    [Fact]
    public void Api_DoesNotReference_DomainDirectly()
    {
        var result = Types.InAssembly(IdentityApiAssembly)
            .Should().NotHaveDependencyOnAny(new[] { "Platform.Identity.Domain" })
            .GetResult();

        Assert.True(result.IsSuccessful, Format(result));
    }

    [Fact]
    public void Contracts_DependsOnlyOn_BuildingBlocksDomain()
    {
        var result = Types.InAssembly(IdentityContractsAssembly)
            .Should().NotHaveDependencyOnAny(new[] { "Platform.Identity.Application", "Platform.Identity.Infrastructure", "Platform.Identity.Api", "Platform.BuildingBlocks.Application", "Platform.BuildingBlocks.Infrastructure", "Platform.BuildingBlocks.Api" })
            .GetResult();

        Assert.True(result.IsSuccessful, Format(result));
    }

    [Fact]
    public void Module_DoesNotReference_OtherModuleInternals()
    {
        var result = Types.InAssembly(IdentityApplicationAssembly)
            .Should().NotHaveDependencyOnAny(new[] { "Platform.Profiles", "Platform.Catalog", "Platform.Geography" })
            .GetResult();

        Assert.True(result.IsSuccessful, Format(result));
    }

    [Fact]
    public void BuildingBlocksInfrastructure_DoesNotDependOn_BuildingBlocksApi()
    {
        var result = Types.InAssembly(BuildingBlocksInfrastructureAssembly)
            .Should().NotHaveDependencyOnAny(new[] { "Platform.BuildingBlocks.Api" })
            .GetResult();

        Assert.True(result.IsSuccessful, Format(result));
    }

    [Fact]
    public void IdentityInfrastructure_DoesNotDependOn_IdentityApi()
    {
        var result = Types.InAssembly(IdentityInfrastructureAssembly)
            .Should().NotHaveDependencyOnAny(new[] { "Platform.Identity.Api" })
            .GetResult();

        Assert.True(result.IsSuccessful, Format(result));
    }

    [Fact]
    public void BuildingBlocksInfrastructure_DoesNotDependOn_Modules()
    {
        var result = Types.InAssembly(BuildingBlocksInfrastructureAssembly)
            .Should().NotHaveDependencyOnAny(new[] { "Platform.Identity", "Platform.Profiles", "Platform.Catalog" })
            .GetResult();

        Assert.True(result.IsSuccessful, Format(result));
    }

    [Fact]
    public void DomainAndApplication_DoNotDependOn_Npgsql()
    {
        foreach (var assembly in new[] { BuildingBlocksDomainAssembly, BuildingBlocksApplicationAssembly, IdentityDomainAssembly, IdentityApplicationAssembly })
        {
            var result = Types.InAssembly(assembly)
                .Should().NotHaveDependencyOnAny(new[] { "Npgsql" })
                .GetResult();

            Assert.True(result.IsSuccessful, Format(result));
        }
    }

    private static string Format(TestResult result)
    {
        if (result.IsSuccessful)
        {
            return string.Empty;
        }

        return string.Join(Environment.NewLine, result.FailingTypes.Select(t => t.FullName));
    }
}
