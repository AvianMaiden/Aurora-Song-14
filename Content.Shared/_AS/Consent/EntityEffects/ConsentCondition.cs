using Content.Shared._Floof.Consent;
using Content.Shared.EntityEffects;
using Content.Shared.Mind;
using Content.Shared.Mind.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Toolshed.Commands.Values;

namespace Content.Shared._AS.Consent.EntityEffects;

public sealed partial class Consent : EntityEffectCondition
{
    public override bool Condition(EntityEffectBaseArgs args)
    {
        if (!args.EntityManager.System<SharedMindSystem>().TryGetMind(args.TargetEntity, out _, out var mind))
            return false;

        if (mind.Session is not { } session)
            return false;

        if (!args.EntityManager.System<SharedConsentSystem>().TryGetConsent(session.UserId, out var settings))
            return false;

        foreach (var toggle in settings.Toggles)
        {
            if (settings is not null && args.EntityManager.System<SharedConsentSystem>().HasConsent(settings, toggle.Key.Id))
                return false;
        }
        return true;
    }

    public override string GuidebookExplanation(IPrototypeManager prototype)
    {
        return string.Empty;
    }
}
