// The old runtime has its own Control enum, which resources already use everywhere. MenuAPI shipping a
// second public one would make every unqualified Control in those resources ambiguous.
global using Control = CitizenFX.Core.Control;
