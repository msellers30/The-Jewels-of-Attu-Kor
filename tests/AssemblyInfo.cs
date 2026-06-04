// The characterization harness redirects Console.In / Console.Out, which is process-global static
// state. Tests must not run concurrently or they would clobber each other's I/O streams.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
