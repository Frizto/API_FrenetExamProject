CREATE PROCEDURE [dbo].[NLog_DeleteEntry_p] (
  @id int,
  @transactionId nvarchar(50)
) AS
BEGIN
  DECLARE @machineName nvarchar(200);
  DECLARE @logged datetime;
  DECLARE @level varchar(5);
  DECLARE @message nvarchar(max);
  DECLARE @logger nvarchar(300);
  DECLARE @properties nvarchar(max);
  DECLARE @exception nvarchar(max);
  DECLARE @transactionId nvarchar(50);

  SELECT
    @machineName = [MachineName],
    @logged = [Logged],
    @level = [Level],
    @message = [Message],
    @logger = [Logger],
    @properties = [Properties],
    @exception = [Exception]
  FROM [dbo].[NLog]
  WHERE [ID] = @id;

  DELETE FROM [dbo].[NLog]
  WHERE [ID] = @id;

  INSERT INTO [dbo].[NLog_Delete] (
    [OriginalID],
    [MachineName],
    [Logged],
    [Level],
    [Message],
    [Logger],
    [Properties],
    [Exception],
	[TransactionId]
  ) VALUES (
    @id,
    @machineName,
    @logged,
    @level,
    @message,
    @logger,
    @properties,
    @exception,
	@transactionId
  );
END
