# XCRM 学习进度

## 当前状态

- 日期：2026-09-22
- 当前课程分支：`lesson/customer-management`
- 已核验提交：`8b5eb4e feat: support customer contact creation`
- 远程状态：本地分支与 `origin/lesson/customer-management` 一致
- 当前未提交业务改动：无

## 已完成并理解

- 完成客户的创建、分页查询、详情修改和启用状态修改。
- 建立 `CustomerContact` 领域实体、EF Core 配置、外键关系和迁移。
- 理解 `DeleteBehavior.Restrict`：客户存在联系人时阻止删除客户。
- 将可选字符串标准化逻辑提取到 `TextNormalizer`，供多个领域实体复用。
- 完成新增客户联系人用例的 DTO、仓储、依赖注入、应用服务和 HTTP 接口。
- 使用应用层重复预检查返回 `409 Conflict`，并使用数据库过滤唯一索引防止并发重复写入。
- 理解迁移文件、已应用迁移和实际数据库结构是三个需要分别核验的状态。

## 已执行验证

- `dotnet build .\backend\XCRM.Api\XCRM.Api.csproj --no-restore`：通过，0 个警告、0 个错误。
- `dotnet test .\backend\XCRM.Application.Tests\XCRM.Application.Tests.csproj --no-build --no-restore`：6/6 通过。
- 新增联系人接口成功场景：返回 `201 Created`，数据写入数据库。
- 重复电话或邮箱场景：返回 `409 Conflict`，进入预期业务分支。
- `AddCustomerContacts` 与 `AddCustomerContactUniqueIndexes` 迁移均已应用。

## 已确认错误与原因

- `ToContactDto(customer)` 参数类型错误：映射方法需要 `CustomerContact`，已改为传入 `contact`。
- 邮箱索引过滤条件曾写成 `[Email}`：C# 将其视为普通字符串，编译不能发现，已在生成迁移前修正为 `[Email]`。
- 多行 `dotnet ef` 命令曾发生参数解析错误，改用单行 `-p` 和 `-s` 参数后解决。

## 下一学习步骤

- 实现查询指定客户联系人列表：`GET /api/customers/{customerId}/contacts`，先从仓储查询契约开始。

## 本机配置

- 本次检查点没有新增需要配置的 User Secrets 键。
