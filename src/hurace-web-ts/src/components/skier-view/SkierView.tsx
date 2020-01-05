import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { Skier } from '../../interfaces/Skier';
import { setStateAsync } from '../../common/stateSetter';
import { getSkiers } from '../../common/api';
import { SkierViewItem } from './SkierViewItem';
import { MasterView } from '../shared/MasterView';
import { SearchContext } from '../shared/MasterView';

const SkierList = styled.div`
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    overflow: auto;
`;

const skierFilter = (skier: Skier, searchTerm: string): boolean =>
    skier.firstName.toLowerCase().includes(searchTerm.toLowerCase()) ||
    skier.lastName.toLowerCase().includes(searchTerm.toLowerCase()) ||
    skier.country.countryName.toLowerCase().includes(searchTerm.toLowerCase());

export const SkierView: React.FC = () => {
    const [skiers, setSkiers] = useState<Skier[] | undefined>(undefined);

    useEffect(() => {
        if (skiers !== undefined) return;
        setStateAsync(setSkiers, getSkiers());
    }, [skiers]);

    return (
        <MasterView createText="Rennläufer erstellen" createUrl="/skier/new">
            <SearchContext.Consumer>
                {search => (
                    <SkierList>
                        {skiers
                            ?.sort((s1, s2) =>
                                s1.lastName.localeCompare(s2.lastName)
                            )
                            .filter(s => skierFilter(s, search))
                            .map(s => (
                                <SkierViewItem key={s.id} skier={s} />
                            ))}
                    </SkierList>
                )}
            </SearchContext.Consumer>
        </MasterView>
    );
};
