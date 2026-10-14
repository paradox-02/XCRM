# XCRM 学习进度

## 当前状态

- 日期：2026-10-14
- 当前课程分支：`lesson/customer-management`
- 已核验提交：`759fb16 feat: list customer follow-up records`
- 远程状态：只读查询 GitHub 已确认远程 `lesson/customer-management` 与本地 HEAD 一致，均为 `759fb16`
- 当前未提交改动：`PagedResult<T>.Map<TResult>`、公共 `PageRequest`、`GetCustomerRequest` 的继承、客户分页 Service 的映射调用，以及本进度文件待提交；跟进分页业务查询尚未开始修改

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
- 完成 `CustomerFollowUp` 实体、EF Core 配置和迁移，以及新增跟进记录的 DTO、仓储、依赖注入、应用服务和 HTTP 接口。
- 跟进记录的客户 ID 只来自路由；选择联系人时按客户 ID 和联系人 ID 查询，防止引用其他客户的联系人。
- 电话快照在创建时保存：手动填写时使用填写值，未填写且选择了联系人时取联系人电话，后续联系人资料变化不改写历史快照。
- 用请求 DTO 的 `[Required]` 校验空内容，用 Service 的空值检查处理客户或联系人不存在；领域构造函数保留核心规则校验。
- 客户创建结果复用 `ToDto(customer)`，减少重复映射；通过短练习理解 `Func<Customer, CustomerDto>` 的输入输出、`Action` 无返回值，以及泛型集合转换中的循环与转换行为。
- 完成指定客户的跟进记录列表查询，仓储按客户 ID 筛选并按创建时间、ID 倒序；Service 复用 `ToFollowUpDto`，Controller 用 `switch` 表达式区分不存在与列表结果。
- 理解 EF Core 查询的延迟执行：筛选、排序在 `ToListAsync` 前构造数据库查询；取得实体列表后的 `Select(ToFollowUpDto)` 在内存中转换数据。
- 实现 `PagedResult<T>.Map<TResult>(Func<T, TResult>)`：逐项调用转换委托，返回新的分页结果并保留总数、页码和每页大小；已接入现有客户分页查询。
- 明确客户查询原本已在数据库中执行 `Skip/Take`，再转换当前页 DTO；新增 `Map` 用于统一结果转换，不能据此认定查询更快或更省内存。
- 提取公共 `PageRequest`，集中定义页码、每页大小的默认值与 `[Range]` 校验；`GetCustomerRequest` 通过继承复用分页属性，仅保留客户查询的关键词。

## 已执行验证

- `dotnet build .\backend\XCRM.Api\XCRM.Api.csproj --no-restore`：通过，0 个警告、0 个错误。
- `dotnet test .\backend\XCRM.Application.Tests\XCRM.Application.Tests.csproj --no-restore`：现有 6 项用户与登录测试通过；这些测试不覆盖跟进记录用例。
- 新增联系人接口成功场景：返回 `201 Created`，数据写入数据库。
- 重复电话或邮箱场景：返回 `409 Conflict`，进入预期业务分支。
- 联系人列表接口：有联系人返回 `200` 和数组，客户不存在返回 `404`，客户存在但无联系人返回 `200 []`。
- 修改联系人接口：成功更新返回 `200`，学习者确认数据库中的资料已更新，并报告后续失败场景测试完成。
- 联系人状态接口：停用和重新启用均返回 `200` 且状态正确；跨客户访问同一联系人返回 `404`。
- `AddCustomerContacts` 与 `AddCustomerContactUniqueIndexes` 迁移均已应用。
- `AddCustomerFollowUps` 迁移已生成并检查；学习者确认数据库更新完成，新增接口成功路径已验证。
- 2026-10-13 验收 `POST /api/customers/{customerId}/follow-ups`：成功返回 `201`、记录 ID 和联系人电话快照；查不到所属联系人返回 `404`；空内容返回 `400` 且错误明确指向 `Content`。截图与当天请求日志的 `201 / 404 / 400` 一致。
- 最后一次 API 项目构建为 0 警告、0 错误；提交后只读核验 GitHub 分支与本地提交一致。
- 跟进列表接口验收：客户 1 有记录返回 `200` 和倒序数组；客户 3 不存在返回 `404`；客户 2 返回 `200`，学习者确认响应体为 `[]`。请求日志与各状态码一致。
- 修正跟进列表返回类型后重新构建：0 警告、0 错误；列表检查点已提交并推送至 `759fb16`，工作区在本次文档同步前干净。
- 2026-10-14 检查 `Map` 循环、分页字段保留和 Service 调用正确；应用层项目构建通过，0 警告、0 错误。
- 公共分页请求接入后，API 项目重新构建通过，0 警告、0 错误；学习者确认客户分页请求 `pageNumber=2&pageSize=1` 和非法页码 `pageNumber=0` 的测试通过，请求日志对应 `200 / 400`。已检查代码按继承属性计算分页并保留结果中的分页字段。

## 已确认错误与原因

- `ToContactDto(customer)` 参数类型错误：映射方法需要 `CustomerContact`，已改为传入 `contact`。
- 邮箱索引过滤条件曾写成 `[Email}`：C# 将其视为普通字符串，编译不能发现，已在生成迁移前修正为 `[Email]`。
- 多行 `dotnet ef` 命令曾发生参数解析错误，改用单行 `-p` 和 `-s` 参数后解决。
- 联系人仓储实现曾误写为 `GetByCustomerByIdAsync`：方法名与接口不一致导致 `CS0535`，已修正为 `GetByCustomerIdAsync`。
- 联系人更新查重曾用 `Id == contactId`，会检查自身而漏掉其他联系人；已改为 `Id != contactId`。
- 跟进请求曾在路由和请求体中重复包含客户 ID；已删除请求 DTO 中的客户 ID，并从路由传入 Service。
- 空内容曾被返回 `null` 或抛出冲突异常，混成 `404` 或 `409`；已改为请求校验返回 `400`，不再用“资源不存在”表示输入无效。
- 收尾时曾误删联系人查不到的判断，出现 `CS8602` 空引用警告并可能在运行时访问 `contact.Id` 失败；已恢复判断，重新构建无警告，联系人不存在场景返回 `404`。
- 跟进列表 Controller 曾声明 `CustomerContactDto`，但实际返回跟进 DTO；`Ok(object)` 允许编译和运行，接口响应模型仍声明错误。已改为 `CustomerFollowUpDto`，并把固定资源路径统一为小写 `follow-ups`。
- `Map` 初次实现曾写成 `result = item.t`；`T` 是类型参数，转换动作来自传入的委托。已改为 `result.Add(convert(item))`，并返回保留分页字段的新结果。
- Scalar 中填写分页值后未启用参数开关，响应仍使用默认页码和每页大小；已核对 `[FromQuery]` 和 `Map` 的参数传递正确，学习者确认理解需要启用参数再发送。

## 下一学习步骤

- 实现跟进记录仓储的分页查询和总数统计，使用共享基础查询统一按客户 ID 筛选，保证计数与分页结果使用相同条件。

## 本机配置

- 本次检查点没有新增需要配置的 User Secrets 键。
