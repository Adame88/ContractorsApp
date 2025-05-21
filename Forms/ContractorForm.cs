using System.ComponentModel;
using ContractorsApp.Validation;
using ContractorsApp.Data;
using ContractorsApp.Models;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using ContractorsApp.Services;

namespace ContractorsApp.Forms
{
    internal partial class ContractorForm : Form
    {
        /// <summary>
        /// A form for adding or editing contractor details, including addresses.
        /// </summary>
        private readonly IContractorService _service;
        internal Contractor Contractor { get; private set; }

        internal ContractorForm(IContractorService service, Contractor contractor)
        {
            _service = service;
            Contractor = contractor;
            InitializeComponent();
            InitializeContractorForm();
        }

        #region Buttons

        private void HandleCancelClick(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Czy na pewno chcesz anulować zmiany?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
        private async void HandleSaveClick(object sender, EventArgs e)
        {
            await SaveContractor();
        }
        private async void HandleDeleteAddressClick(object sender, EventArgs e)
        {
            try
            {
                // get address from grid
                var selectedAddress = FetchSelectedAddress();
                if (selectedAddress == null) return;

                // are you sure?
                if (!PromptAddressDeletionConfirmation()) return;


                await RemoveAddressAsync(selectedAddress, Contractor);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas usuwania adresu: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion


        #region Helpers

        #region InitForm

        private void InitializeContractorForm()
        {
            txtName.Text = Contractor.CompanyName;
            txtTaxNumber.Text = Contractor.TaxNumber;
            txtREGON.Text = Contractor.REGON;

            SetupAddressDataGridView();
            dgvAddresses.DataSource = new BindingList<Address>(Contractor.Addresses);
        }
        private void SetupAddressDataGridView()
        {
            dgvAddresses.AutoGenerateColumns = false;

            dgvAddresses.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Address.Street),
                HeaderText = GetDisplayName(typeof(Address), nameof(Address.Street))
            });
            dgvAddresses.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Address.City),
                HeaderText = GetDisplayName(typeof(Address), nameof(Address.City))
            });
            dgvAddresses.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Address.PostalCode),
                HeaderText = GetDisplayName(typeof(Address), nameof(Address.PostalCode))
            });
            dgvAddresses.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(Address.IsMainAddress),
                HeaderText = GetDisplayName(typeof(Address), nameof(Address.IsMainAddress))
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
        #endregion

        #region SaveContractor&ValidateContractor
        private async Task SaveContractor()
        {
            try
            {
                Contractor.CompanyName = txtName.Text;
                Contractor.TaxNumber = txtTaxNumber.Text;
                Contractor.REGON = txtREGON.Text;

                if (ValidateContractor(Contractor))
                {
                    Contractor.Addresses = ((BindingList<Address>)dgvAddresses.DataSource).ToList();
                    await _service.AddOrUpdateContractorAsync(Contractor);
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił nieoczekiwany błąd: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool ValidateContractor(Contractor contractor)
        {
            var validator = new ContractorFluentValidation();
            var validationResult = validator.Validate(contractor);

            if (!validationResult.IsValid)
            {
                // Jeśli walidacja nie przeszła, wyświetlamy komunikaty o błędach
                string errorMessage = string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage));
                MessageBox.Show(errorMessage, "Błąd walidacji", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        #endregion

        #region RemoveAddress
        private Address? FetchSelectedAddress()
        {
            if (dgvAddresses.CurrentRow == null)
            {
                MessageBox.Show("Nie wybrano żadnego adresu do usunięcia.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            var selectedRowIndex = dgvAddresses.CurrentRow.Index;
            var addresses = (BindingList<Address>)dgvAddresses.DataSource;
            return addresses[selectedRowIndex];
        }

        private bool PromptAddressDeletionConfirmation()
        {
            var result = MessageBox.Show("Czy na pewno chcesz usunąć wybrany adres?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        private async Task RemoveAddressAsync(Address address, Contractor contractor)
        {
            if (address.IsMainAddress)
            {
                MessageBox.Show("Nie można usunąć głównego adresu", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //remove from db and update db
            await _service.DeleteAddressAsync(address.AddressID);
            var addresses = (BindingList<Address>)dgvAddresses.DataSource;
            addresses.Remove(address);
            await _service.AddOrUpdateContractorAsync(contractor);

            //remove from grid

        }
        #endregion
        #endregion
    }
}
