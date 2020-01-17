import { useState, useEffect } from 'react';
import { SelectValue } from '../../interfaces/SelectValue';
import {
    getAllCountries,
    getAllGenders,
    getAllDisciplines,
    getSkierById,
    getDisciplinesForSkier
} from '../../common/api';
import { SkierFormValues } from '../../types/forms/skier-form';
import { useCallState } from '../../hooks/useCallState';
import { ApiState } from '../../interfaces/ApiState';

export const useSkierForm = (
    skierId?: number
): [
    ApiState,
    SkierFormValues | undefined,
    SelectValue[] | undefined,
    SelectValue[] | undefined,
    SelectValue[] | undefined
] => {
    const [initialFormValue, setInitialFormValue] = useState<SkierFormValues>();
    const [countries, setCountries] = useState<SelectValue[]>();
    const [genders, setGenders] = useState<SelectValue[]>();
    const [disciplines, setDisciplines] = useState<SelectValue[]>([]);
    const callState = useCallState();

    useEffect(() => {
        const loadData = async () => {
            callState.setLoading(true);
            callState.setError(undefined);
            try {
                let countries = (await getAllCountries()).map(c => ({
                    label: c.countryName,
                    value: c.id
                }));

                let genders = (await getAllGenders()).map(g => ({
                    label: g.genderDescription,
                    value: g.id
                }));

                let disciplines = (await getAllDisciplines()).map(d => ({
                    label: d.disciplineName,
                    value: d.id
                }));

                setCountries(countries);
                setGenders(genders);
                setDisciplines(disciplines);
                if (skierId !== undefined) {
                    let skier = await getSkierById(skierId!);
                    let skierDisciplines = await getDisciplinesForSkier(
                        skierId!
                    );

                    if (skier === undefined) return;

                    setInitialFormValue({
                        firstName: skier.firstName,
                        lastName: skier.lastName,
                        dateOfBirth: skier.dateOfBirth,
                        imageUrl: skier.imageUrl,
                        retired: skier.retired,
                        selectedCountry: countries?.find(
                            c => c.value === skier.countryId
                        ),
                        selectedDisciplines: disciplines?.filter(d =>
                            skierDisciplines.some(sk => sk.id === d.value)
                        ),
                        selectedGender: genders?.find(
                            g => g.value === skier.genderId
                        )
                    });
                } else
                    setInitialFormValue({
                        firstName: '',
                        lastName: '',
                        retired: false,
                        imageUrl: '',
                        selectedGender: undefined,
                        dateOfBirth: new Date(),
                        selectedCountry: undefined,
                        selectedDisciplines: []
                    });
            } catch (error) {
                callState.setError(error);
            } finally {
                callState.setLoading(false);
            }
        };

        loadData();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return [callState, initialFormValue, genders, countries, disciplines];
};
