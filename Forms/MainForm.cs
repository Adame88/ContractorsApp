using System.ComponentModel.DataAnnotations;
using System.Data;
using ContractorsApp.Data;
using ContractorsApp.Forms;
using ContractorsApp.Models;
using ContractorsApp.Services;

namespace ContractorsApp
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Contains the auto-generated designer code for the MainForm layout and controls.
        /// </summary>
        private readonly IContractorRepository _repo;
        private readonly IContractorService _service;
        public MainForm()
        {
            _repo = new ContractorRepository();
            _service = new ContractorService(_repo) as IContractorService;
            InitializeComponent();
            LoadContractorsWithFiltersAsync();
            txtNameFilter.TextChanged += async (s, e) => await LoadContractorsWithFiltersAsync();
            txtTaxNumberFilter.TextChanged += async (s, e) => await LoadContractorsWithFiltersAsync();
        }


        #region Buttons

        private void HandleAddContractorClick(object sender, EventArgs e)
        {
            try
            {
                using (var addForm = new ContractorForm(_service, new Contractor()))
                {
                    if (addForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadContractorsWithFiltersAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas dodawania kontrahenta: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void HandleEditContractorClick(object sender, EventArgs e)
        {
            try
            {
                var contractor = await FetchSelectedContractorAsync();
                if (contractor == null)
                {
                    MessageBox.Show("Nie znaleziono wybranego kontrahenta.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var form = new ContractorForm(_service, contractor))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        await LoadContractorsWithFiltersAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas edycji kontrahenta: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void HandleDeleteContractorClick(object sender, EventArgs e)
        {
            try
            {

                var result = MessageBox.Show("Czy na pewno chcesz usunąć wybranego kontrahenta?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {

                    var contractor = await FetchSelectedContractorAsync();
                    await _repo.RemoveContractorByIdAsync(contractor.ContractorId);

                    await LoadContractorsWithFiltersAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas usówania kontrahenta: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Helpers

        private async Task LoadContractorsWithFiltersAsync()
        {
            try
            {
                var name = txtNameFilter.Text.Trim();
                var taxNumber = txtTaxNumberFilter.Text.Trim();

                var contractors = await _service.FetchContractorsAsync(name, taxNumber);

                var contractorData = contractors.Select(c => new
                {
                    c.ContractorId,
                    c.CompanyName,
                    c.TaxNumber,
                    c.REGON,
                    Street = c.MainAddress?.Street ?? "",
                    City = c.MainAddress?.City ?? "",
                    PostalCode = c.MainAddress?.PostalCode ?? ""
                }).ToList();

                dataGridView.DataSource = contractorData;

                SetupContractorDataGridViewColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas ładowania kontrahentów: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SetupContractorDataGridViewColumns()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.Clear();

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Contractor.ContractorId),
                HeaderText = GetDisplayName(typeof(Contractor), nameof(Contractor.ContractorId)),
                Name = "ContractorId"
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Contractor.CompanyName),
                HeaderText = GetDisplayName(typeof(Contractor), nameof(Contractor.CompanyName))
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Contractor.TaxNumber),
                HeaderText = GetDisplayName(typeof(Contractor), nameof(Contractor.TaxNumber))
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Contractor.REGON),
                HeaderText = GetDisplayName(typeof(Contractor), nameof(Contractor.REGON))
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Street",
                HeaderText = GetDisplayName(typeof(Address), nameof(Address.Street))
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "City",
                HeaderText = GetDisplayName(typeof(Address), nameof(Address.City))
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PostalCode",
                HeaderText = GetDisplayName(typeof(Address), nameof(Address.PostalCode))
            });
        }
        private string GetDisplayName(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName);
            var displayAttribute = property?.GetCustomAttributes(typeof(DisplayAttribute), false)
                .Cast<DisplayAttribute>()
                .FirstOrDefault();

            return displayAttribute?.Name ?? propertyName;
        }
        private async Task<Contractor?> FetchSelectedContractorAsync()
        {
            if (dataGridView.CurrentRow == null)
            {
                MessageBox.Show("Nie wybrano żadnego kontrahenta.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            try
            {
                // Pobierz ContractorId z wybranego wiersza
                var contractorId = (int)dataGridView.CurrentRow.Cells["ContractorId"].Value;

                // Pobierz kontrahenta z bazy danych
                return await _repo.FetchContractorByIdAsync(contractorId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas pobierania kontrahenta: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        #endregion

    }
}
