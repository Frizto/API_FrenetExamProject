CREATE PROCEDURE [dbo].[NLog_AddEntry_p] (
  @machineName nvarchar(200),
  @logged datetime,
  @level varchar(5),
  @message nvarchar(max),
  @logger nvarchar(300),
  @properties nvarchar(max),
  @exception nvarchar(max),
  @transactionId nvarchar(50),
  @entityId nvarchar(50)
) AS
BEGIN
  INSERT INTO [dbo].[NLog] (
    [MachineName],
    [Logged],
    [Level],
    [Message],
    [Logger],
    [Properties],
    [Exception],
	[TransactionId],
	[EntityId]
  ) VALUES (
    @machineName,
    @logged,
    @level,
    @message,
    @logger,
    @properties,
    @exception,
	@transactionId,
	@entityId
  );

  INSERT INTO [dbo].[NLog_Create] (
    [MachineName],
    [Logged],
    [Level],
    [Message],
    [Logger],
    [Properties],
    [Exception],
	[TransactionId],
	[EntityId]
  ) VALUES (
    @machineName,
    @logged,
    @level,
    @message,
    @logger,
    @properties,
    @exception,
	@transactionId,
	@entityId
  );
END


