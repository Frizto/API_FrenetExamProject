CREATE PROCEDURE [dbo].[NLog_DeleteEntry_p] (
  @id int,
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
  UPDATE [dbo].[NLog]
  SET
    [MachineName] = @machineName,
    [Logged] = @logged,
    [Level] = @level,
    [Message] = @message,
    [Logger] = @logger,
    [Properties] = @properties,
    [Exception] = @exception,
	[TransactionId] = @transactionId,
	[EntityId] = @entityId
  WHERE [EntityId] = @entityId;

  INSERT INTO [dbo].[NLog_Delete] (
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