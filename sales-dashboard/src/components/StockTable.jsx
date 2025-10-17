function StockTable({ data }) {
  return (
    <div className="table-container">
      <table>
        <thead>
          <tr>
            <th>Armazém</th>
            <th>SKU</th>
            <th>Produto</th>
            <th>Quantidade</th>
            <th>Mínimo</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          {data.map((item, index) => (
            <tr key={index}>
              <td>{item.warehouseId}</td>
              <td><code>{item.sku}</code></td>
              <td>{item.productName}</td>
              <td className="qty">{item.quantityAvailable}</td>
              <td>{item.minimumLevel}</td>
              <td>
                <span className={`badge ${item.stockStatus === 'Out of Stock' ? 'red' : 'yellow'}`}>
                  {item.stockStatus}
                </span>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

export default StockTable
