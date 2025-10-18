/**
 * Componente de tabela para exibir produtos com stock baixo.
 * Mostra informações de armazém, produto, quantidades e status visual.
 */
function StockTable({ data }) {
  return (
    <div className="table-container">
      <table>
        {/* Cabeçalho da tabela */}
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
        
        {/* Corpo da tabela com dados */}
        <tbody>
          {data.map((item, index) => (
            <tr key={index}>
              {/* ID do armazém */}
              <td>{item.warehouseId}</td>
              
              {/* SKU formatado como código */}
              <td><code>{item.sku}</code></td>
              
              {/* Nome do produto */}
              <td>{item.productName}</td>
              
              {/* Quantidade disponível (com classe para styling) */}
              <td className="qty">{item.quantityAvailable}</td>
              
              {/* Nível mínimo de stock */}
              <td>{item.minimumLevel}</td>
              
              {/* Badge colorida baseada no status */}
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
