# XCRM 学习进度

## 当前状态

- 日期：2026-09-30
- 当前课程分支：`lesson/customer-management`
- 已核验提交：`dcab940 feat: update customer contact status`
- 远程状态：本地分支与 `origin/lesson/customer-management` 一致
- 当前未提交改动：无业务代码改动；仅本进度文件待提交

## 已完成并理解

- 完成客户的创建、分页查询、详情修改和启用状态修改。
- 建立 `CustomerContact` 领域实体、EF Core 配置、外键关系和迁移。
- 理解 `DeleteBehavior.Restrict`：客户存在联系人时阻止删除客户。
- 将可选字符串标准化逻辑提取到 `TextNormalizer`，供多个领域实体复用。
- 完成新增客户联系人用例的 DTO、仓储、依赖注入、应用服务和 HTTP 接口。
- 完成查询指定客户联系人列表，并区分客户不存在、空列表和有联系人三个结果。
- 完成修改客户联系人资料用例；按客户和联系人 ID 限定更新对象，查重时排除当前联系人。
- 完成客户联系人启用和停用；复用领域实体的状态方法，并按客户 ID 限定操作对象。
- 使用应用层重复预检查返回 `409 Conflict`，并使用数据库过滤唯一索引防止并发重复写入。
- 理解迁移文件、已应用迁移和实际数据库结构是三个需要分别核验的状态。

## 已执行验证

- `dotnet build .\backend\XCRM.Api\XCRM.Api.csproj --no-restore`：通过，0 个警告、0 个错误。
- `dotnet test .\backend\XCRM.Application.Tests\XCRM.Application.Tests.csproj --no-build --no-restore`：6/6 通过。
- 新增联系人接口成功场景：返回 `201 Created`，数据写入数据库。
- 重复电话或邮箱场景：返回 `409 Conflict`，进入预期业务分支。
- 联系人列表接口：有联系人返回 `200` 和数组，客户不存在返回 `404`，客户存在但无联系人返回 `200 []`。
- 修改联系人接口：成功更新返回 `200`，学习者确认数据库中的资料已更新，并报告后续失败场景测试完成。
- 联系人状态接口：停用和重新启用均返回 `200` 且状态正确；跨客户访问同一联系人返回 `404`。
- `AddCustomerContacts` 与 `AddCustomerContactUniqueIndexes` 迁移均已应用。

## 已确认错误与原因

- `ToContactDto(customer)` 参数类型错误：映射方法需要 `CustomerContact`，已改为传入 `contact`。
- 邮箱索引过滤条件曾写成 `[Email}`：C# 将其视为普通字符串，编译不能发现，已在生成迁移前修正为 `[Email]`。
- 多行 `dotnet ef` 命令曾发生参数解析错误，改用单行 `-p` 和 `-s` 参数后解决。
- 联系人仓储实现曾误写为 `GetByCustomerByIdAsync`：方法名与接口不一致导致 `CS0535`，已修正为 `GetByCustomerIdAsync`。
- 联系人更新查重曾用 `Id == contactId`，会检查自身而漏掉其他联系人；已改为 `Id != contactId`。

## 下一学习步骤

- 明确客户跟进记录的归属规则与最小字段，为下一个 CRM 业务用例建模。

## 本机配置

- 本次检查点没有新增需要配置的 User Secrets 键。
