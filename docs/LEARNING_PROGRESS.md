# XCRM 学习进度

## 当前状态

- 日期：2026-10-16
- 当前课程分支：`lesson/customer-management`
- 已核验提交：`95be4e2 feat: return follow-up location after creation`
- 远程状态：只读查询 GitHub 已确认远程 `lesson/customer-management` 与本地 HEAD 一致，均为 `95be4e2`
- 当前未提交改动：跟进列表筛选请求 DTO、Controller 与 Service 的请求类型调整，以及学习进度文件；输入校验已核验，仓储尚未使用关键词和时间条件

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
- 实现跟进仓储 `CountAsync` 和 `GetPageAsync`，共用按客户 ID 筛选的 `BuildQuery`；计数使用 EF Core 异步查询，分页先按创建时间、ID 倒序，再执行 `Skip/Take`。
- 跟进 Service 和 GET 接口已接入公共 `PageRequest`、计数及分页仓储方法、`PagedResult<T>.Map`；成功响应由数组改为包含 `items` 和分页信息的对象。
- 完成跟进分页的正常页、越界页、客户无记录、客户不存在和页码无效验收；删除跟进仓储已无调用的旧全列表查询声明与实现。
- 完成单条跟进详情的仓储和 Service：复用 `BuildQuery(customerId)` 后按跟进 ID 查询，查不到返回 `null`，查到复用 `ToFollowUpDto`，不额外查询客户或改写电话快照。
- 完成单条跟进详情 GET 接口：Controller 使用 `switch` 表达式将 DTO 映射为 `200`、将 `null` 映射为 `404`；按客户 ID 限定记录归属，成功与失败场景均已验收。
- 新增跟进接口使用 `CreatedAtAction` 返回 `201`、DTO 和新记录的 `Location`；使用 `nameof` 指定详情 Action，以匿名对象传递客户 ID 和跟进 ID，已验证创建后能够按该地址读取相同记录。
- 跟进筛选请求继承 `PageRequest`，增加关键词和可空 `DateTimeOffset` 起止时间；实现 `IValidatableObject`，用空属性模式取出非空值，并用 `yield return` 提供 `ValidationResult`。开始时间大于或等于结束时间时返回验证错误。

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
- 公共分页输入和结果转换检查点已提交并推送至 `a33c153`；已只读核验 GitHub 分支，工作区在本次进度同步前干净。
- 跟进仓储计数和分页实现经代码检查，API 项目构建通过，0 警告、0 错误。
- 跟进分页成功场景：客户 1、第 2 页、每页 2 条返回 `200`，记录 ID 为 4、3，`totalCount=6`、`pageNumber=2`、`pageSize=2`，截图与查询逻辑一致。
- 三层分页返回类型已统一；使用 `dotnet build .\backend\XCRM.Api\XCRM.Api.csproj --no-restore -p:OutputPath=bin/verification/` 避开调试程序占用输出文件，构建通过，0 警告、0 错误。
- 跟进分页边界验收：客户 1 第 4 页每页 2 条返回 `200`、`items=[]`、`totalCount=6`；客户 2 第 1 页返回 `200`、空数组、总数 0；客户 3 返回 `404`；客户 1 页码 0 返回 `400`，错误明确指向 `PageNumber`。四张截图与当天请求日志的 `200 / 200 / 404 / 400` 一致。
- 删除旧全列表查询后再次用独立输出目录构建通过，0 警告、0 错误；最终 diff 检查通过，改动限于本次分页功能及学习进度。
- 跟进分页检查点已提交并推送至 `6b609b7`；已核对提交 diff 与验收代码一致，并只读确认 GitHub 学习分支与本地 HEAD 一致，核验时工作区干净。
- 学习者确认详情查询的仓储和 Service 编译通过；实际 diff 核验两个 ID 共同限定查询、取消令牌传递和 DTO 映射正确；API 项目使用独立输出目录构建通过，0 警告、0 错误。详情 HTTP 行为尚待验收。
- 详情 HTTP 首次验收：客户 1 的跟进 1 返回 `200` 和正确 DTO；客户 1 的跟进 7、客户 2 的跟进 1 均返回 `204`，与当天请求日志一致。当时未达到预期 `404`，后续已修正。
- 详情 HTTP 修正后验收：客户 2 的跟进 1 返回 `404`，客户 1 的跟进 7 返回 `404`，客户 1 的跟进 1 返回 `200` 且 DTO 的 `id=1`、`customerId=1`。三张截图与当天请求日志一致；已检查两个 ID 联合筛选和 Controller 空值分支，API 独立输出目录构建通过，0 警告、0 错误，最终 diff 检查通过。
- 详情查询检查点已提交并推送至 `e06701f`；已只读核验 GitHub 学习分支与本地 HEAD 一致，提交范围与验收代码一致，核验时工作区干净。
- 2026-10-16 核验新增跟进响应：`CreatedAtAction` 指向详情 Action，路由值包含客户 ID 和新记录 ID；API 独立输出目录构建通过，0 警告、0 错误。POST 返回 `201`，响应体 `id=7`、`customerId=1`，Location 路径为 `/api/customers/1/follow-ups/7`；随后详情 GET 返回 `200`，ID、客户 ID、电话快照和内容与新增响应一致。截图与当天请求日志的 `201 / 200` 一致，最终 diff 检查通过。
- `CreatedAtAction` 检查点已提交并推送至 `95be4e2`；只读核验 GitHub 学习分支与本地 HEAD 一致，核验时工作区干净。
- 跟进筛选输入验收：截图确认未填时间和正常范围返回 `200`，时间反转返回带自定义提示的 `400`；将比较修正为 `>=` 后，学习者确认相同时间返回 `400`，代码检查及最新请求日志一致。筛选输入已接入 Controller 和 Service，数据库查询仍只按客户 ID 筛选。
- 筛选输入修正后 API 独立输出目录构建通过，0 个错误、4 条 NuGet 安全警告；警告来自 API 和 Infrastructure 使用的 `System.Security.Cryptography.Pkcs` 9.0.4（NU1902、NU1903），依赖来源和更新方案需单独核验，本轮未更改依赖。

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
- 跟进计数初次实现虽标记 `async`，但调用同步 `Count()`，且未传递取消令牌；已改为 `await CountAsync(cancellationToken)`。分页初次实现缺少排序，已在 `Skip/Take` 前加入创建时间和 ID 倒序。
- 跟进分页接入时只增加了请求参数，接口、Service 和 Controller 的返回声明仍为列表；`Map` 返回完整分页对象，导致无法隐式转换。已统一为 `PagedResult<CustomerFollowUpDto>` 对应的返回类型。
- 核验构建曾因运行中的 API 占用输出 DLL 而复制失败；使用独立的 `bin/verification/` 输出目录完成核验，未修改业务代码或中断调试。
- 详情 Controller 曾直接执行 `Ok(followUp)`，未处理 Service 返回的 `null`；ASP.NET Core 默认将空对象响应处理为 `204`。已在 Controller 显式将 `null` 映射为 `NotFound()`，复测两个失败场景均返回 `404`，仓储和 Service 的查询逻辑保持不变。
- 时间校验初次仅判断两项有值，会误拒绝正常范围；增加比较时曾使用 `>`，相等时条件为 false，仍返回 `200`。已改为 `from >= to`，与“开始时间必须早于结束时间”的规则一致。

## 下一学习步骤

- 将跟进筛选输入标准化并转换为 UTC，接入仓储的共享查询条件，使关键词、时间范围的计数与分页保持一致。

## 本机配置

- 本次检查点没有新增需要配置的 User Secrets 键。
